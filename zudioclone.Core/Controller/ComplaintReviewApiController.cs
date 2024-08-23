using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using NPoco.fastJSON;
using Umbraco.Cms.Infrastructure.Scoping;
using zudioclone.Core.Controllers;
using zudioclone.models.Models.Viewmodels;

namespace zudioclone.Core.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintReviewApiController : ControllerBase
    {
        private readonly IScopeProvider _scopeProvider;
        private readonly ILogger<ComplaintReviewApiController> _logger;

        public ComplaintReviewApiController(IScopeProvider scopeProvider, ILogger<ComplaintReviewApiController> logger)
        {
            _scopeProvider = scopeProvider;
            _logger = logger;
        }

     
        [HttpPost("PostComplaintReview")]
        public IActionResult PostComplaintReview([FromBody] ComplaintReviewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            try
            {



                using (var scope = _scopeProvider.CreateScope())
                {
                    var database = scope.Database;

                    // Automatically set the DateSubmitted property
                    model.DateSubmitted = DateTime.UtcNow;

                    // Create the SQL query for insertion
                    var sql = @"
                INSERT INTO ComplaintReviews (Name, Email, Complaint, Review, DateSubmitted)
                VALUES (@Name, @Email, @Complaint, @Review, @DateSubmitted)";

                    // Execute the SQL query
                    database.Execute(sql, new
                    {
                        Name = model.Name,
                        Email = model.Email,
                        Complaint = model.Complaint,
                        Review = model.Review,
                        DateSubmitted = model.DateSubmitted
                    });

                    scope.Complete();
                }
            }
            catch (Exception ex) {

                // Log the exception details (assuming you have a logging mechanism)
                _logger.LogError(ex, "An error occurred while registering the member.");

                // Add a generic error message to the ModelState
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");


            }


            return Ok("Complaint or Review submitted successfully.");
        }

        

        [HttpGet("GetComplaintReviews")]
        public IActionResult GetComplaintReviews()
        {
            List<ComplaintReviewModel> reviews;

            using (var scope = _scopeProvider.CreateScope())
            {
                var database = scope.Database;

                // Fetch all complaint reviews from the database
                reviews = database.Query<ComplaintReviewModel>("SELECT * FROM ComplaintReviews").ToList();

                scope.Complete();
            }

            // Return the data as JSON for the frontend
            return Ok(reviews);
        }


    }
}
