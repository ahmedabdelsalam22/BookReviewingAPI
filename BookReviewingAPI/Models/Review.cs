using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookReviewingAPI.Models
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(200,MinimumLength =10,ErrorMessage = "HeadLine must be between 10 and 200 characters")]
        public string HeadLine { get; set; }
        [Required]
        [StringLength(2000, MinimumLength = 50, ErrorMessage = "Review needs to be between 50 and 2000 characters")]
        public string ReviewText { get; set; }
        [Required]
        [Range(1,5,ErrorMessage ="Rating must be between 1 and 5 stars")]
        public int Rating { get; set; }

        //public int BookId { get; set; } // foreignKey
        public virtual Book Book { get; set; }

        //public int ReviewerId { get; set; } // foreignKey
        public virtual Reviewer Reviewer { get; set; }
    }
}
