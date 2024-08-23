using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zudioclone.models.Models.Viewmodels
{
    public class ComplaintReviewModel
    {


        public int Id { get; set; } // Primary Key

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "not exceed more than 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "invalid email id")]
        public string Email { get; set; }

        public string Complaint { get; set; }

        [Required(ErrorMessage = "pls enter review")]
        public string Review { get; set; }

        [Required]
        public DateTime DateSubmitted { get; set; }
    }

}
