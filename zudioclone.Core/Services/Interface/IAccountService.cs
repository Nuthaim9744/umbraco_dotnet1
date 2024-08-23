

using System.Threading.Tasks;
using zudioclone.models.Models.Viewmodels;
using Microsoft.AspNetCore.Identity;

namespace zudioclone.Core.Services.Interface
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterMemberAsync(RegisterViewModel model);
        Task<SignInResult> LoginMemberAsync(LoginViewModel model);
        Task LogoutMemberAsync();
    }
}