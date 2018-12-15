using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace thebeltexam.Models
{
  public class Activity
    {
        [Key]
        public int ActivityId {get;set;}

        [Required]
        [Display(Name="Title:")]
        [MinLength(2, ErrorMessage="Title must be 2 characters or longer!")]
        public string Title { get; set; }
        
        [Required]
        [Display(Name="Duration:")]
        public int Duration { get; set; }

        [Required]
        public string DurationType { get; set; }
        
        [Required]
        [FutureDate]
        [Display(Name="Date:")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name="Time:")]
        public DateTime Time { get; set; }

        [Required]
        [Display(Name="Description:")]
        [MinLength(10, ErrorMessage="Description must be 10 characters or longer!")]
        public string Description { get; set; }
        public int? CoordinatorId { get; set; }
        public User Coordinator {get;set;}
        public List<Participant> Participants {get; set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

    }
}