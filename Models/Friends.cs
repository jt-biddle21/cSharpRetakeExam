using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace cSharpRetakeExam.Models
{
    public class Friend : BaseEntity
    {
        [Key]
        public int Id {get; set;}

        public string FriendName {get; set;}
        
        public string FriendDescription {get; set;}
        public int InviteId {get; set;}
        public int Users_Id {get; set;}
        public User user {get; set;}
    }
}