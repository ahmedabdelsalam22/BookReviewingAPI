using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
        public virtual ICollection<Author> Authors { get; set; }
    }
}
