using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using JsonIgnoreAttribute = System.Text.Json.Serialization.JsonIgnoreAttribute;


namespace BookReviewingAPI.Models
{
    public class Country
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50,ErrorMessage ="Country name can't be more than 50 characters")]
        public string Name { get; set; }
        [JsonIgnore]
        public virtual ICollection<Author>? Authors { get; set; }
    }
}
