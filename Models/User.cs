using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace thebeltexam.Models
{
  public class User
    {
        [Key]
        public int UserId {get;set;}

        [Required]
        [MinLength(3)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        [Display(Name = "First Name:")]
        public string FirstName {get;set;}

        [Required]
        [MinLength(3)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        [Display(Name = "Last Name:")]
        public string LastName {get;set;}

        [Required]
        [EmailAddress]
        [Display(Name = "Email:")]
        public string Email {get;set;}

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$", ErrorMessage = "Password must be minimum eight characters, at least one letter, one number and one special character")]
        [Display(Name = "Password:")]
        public string Password {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password:")]
        public string Confirm {get;set;}

    }
}