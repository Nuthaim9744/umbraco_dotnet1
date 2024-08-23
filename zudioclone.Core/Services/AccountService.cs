


using System.Threading.Tasks;
using zudioclone.models.Models.Viewmodels;
using Microsoft.AspNetCore.Identity;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common.Security;
using zudioclone.Core.Services.Interface;
using Umbraco.Cms.Core.Models;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using zudioclone.Core.Controllers;


namespace zudioclone.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMemberManager _memberManager;
        private readonly IMemberService _memberService;
        private readonly IMemberSignInManager _memberSignInManager;
        private readonly IMemberGroupService _memberGroupService;
        private readonly ILogger<AccountService> _logger;

        public AccountService(
            IMemberManager memberManager,
            IMemberService memberService,
            IMemberSignInManager memberSignInManager,
            IMemberGroupService memberGroupService,
            ILogger<AccountService> logger)
        {
            _memberManager = memberManager;
            _memberService = memberService;
            _memberSignInManager = memberSignInManager;
            _memberGroupService = memberGroupService;
            _logger = logger;
        }


        public async Task<IdentityResult> RegisterMemberAsync(RegisterViewModel model)
        {

            try
            {


                // Create a new identity user
                var identityUser = MemberIdentityUser.CreateNew(model.Email, model.Email, "Member", true, model.Name);

                // Attempt to create the user in the membership system
                var result = await _memberManager.CreateAsync(identityUser, model.Password);

                if (result.Succeeded)
                {
                    // Retrieve the newly created member
                    var member = _memberService.GetByKey(identityUser.Key);
                    if (member != null)
                    {
                        // Set the member's name
                        member.Name = model.Name;

                        // Check if the "Users" group exists
                        var usersGroup = _memberGroupService.GetByName("Users");
                        if (usersGroup == null)
                        {
                            // Create the "Users" group if it doesn't exist
                            usersGroup = new MemberGroup { Name = "Users" };
                            _memberGroupService.Save(usersGroup);
                        }

                        // Add the member to the "Users" group
                        _memberService.AssignRole(member.Id, usersGroup.Name);

                        // Save the updated member information
                        _memberService.Save(member);

                        // Note: We're not automatically signing in the user after registration
                        // This line is commented out to prevent automatic login
                        // await _memberSignInManager.SignInAsync(identityUser, isPersistent: false);
                    }
                }

                // Return the result of the registration attempt
                return result;
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while registering the member.");

                // Return a failed IdentityResult with a generic error message
                return IdentityResult.Failed(new IdentityError { Description = "An unexpected error occurred. Please try again later." });



            }
        }





        public async Task<SignInResult> LoginMemberAsync(LoginViewModel model)
        {
            return await _memberSignInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);
        }

        public async Task LogoutMemberAsync()
        {
            await _memberSignInManager.SignOutAsync();
        }
    }
}


