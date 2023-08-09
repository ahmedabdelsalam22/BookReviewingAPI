using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using JsonIgnoreAttribute = System.Text.Json.Serialization.JsonIgnoreAttribute;


namespace BookReviewingAPI.Models
{
    public class Reviewer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "First Name must be up to 100 characters in length")]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(200, ErrorMessage = "Last Name must be up to 200 characters in length")]
        public string LastName { get; set; }
        [JsonIgnore]
        public virtual ICollection<Review>? Reviews { get; set; }
    }
}
