using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarBank.WebApi.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long? Id                         { get; set; }
        [Required]
        public string FirstName                 { get; set; }
        public string MiddleName                { get; set; }
        [Required]
        public string LastName                  { get; set; }
        // reference to User. Each customer has a UserId
        [Required]
        public string IdentityId                { get; set; }
        public ApplicationUser Identity         { get; set; } // navigation property
    }
}
