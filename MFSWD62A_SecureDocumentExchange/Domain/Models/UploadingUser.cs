using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Domain.Models
{
    /// <summary>
    /// This model class embodies a user who wishes to upload a document via the web application.
    /// </summary>
    public class UploadingUser : IdentityUser
    {
        /// <summary>
        /// This property is used to store the first name of the user.
        /// A not null constraint has been assigned through the Required data annotation attribute.
        /// Values must contain at least 2 characters but no more than 25 characters.
        /// </summary>
        [Required, MinLength(2), MaxLength(25)]
        public string FirstName { get; set; }

        /// <summary>
        /// This property is used to store the last name of the user.
        /// A not null constraint has been assigned through the Required data annotation attribute.
        /// Values must contain at least 2 characters but no more than 50 characters.
        /// </summary>
        [Required, MinLength(2), MaxLength(50)]
        public string LastName { get; set; }
    }
}
