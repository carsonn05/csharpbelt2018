using System.ComponentModel.DataAnnotations;

namespace thebeltexam.Models
{
  public class Participant
    {
        [Key]
        public int ParticipantId {get; set;}
        public int UserId {get; set;}
        public User User {get; set;}
        public int ActivityId {get; set;}
        public Activity Activity {get; set;}
    }
}