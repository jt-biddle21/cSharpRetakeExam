using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace cSharpRetakeExam.Models
{
    public class User : BaseEntity
    {
        [Key]
        public int Id {get; set;}

        [Required]
        [RegularExpression(@"^[a-zA-Z ]+$")]
        public string Name {get; set;}
        
        [Required]
        public string Email {get; set;}
        
        [Required]
        public string Description {get; set;}

        [Required]
        [MinLength(8)]
        public string Password {get; set;}

        public IEnumerable<Friend> friends {get; set;}

        public User()
        {
            friends = new List<Friend>();
        }
    }
}