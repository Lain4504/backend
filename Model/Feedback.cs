namespace BackEnd.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace OnlineBookShop.Models
    {
        [Table("Feedback")]
        public class Feedback
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public long Id { get; set; }

            public FeedbackState State { get; set; }

            [ForeignKey("BookId")]
            public long? BookId { get; set; }
            public virtual Book Book { get; set; }

            [Required(ErrorMessage = "The comment is required")]
            public string Comment { get; set; }

            public string Answer { get; set; }

            public DateTime CreatedAt { get; set; }

            [ForeignKey("UserId")]
            public long? UserId { get; set; }
            public virtual User User { get; set; }
        }

        public enum FeedbackState
        {
            Pending,
            Approved,
            Rejected
        }

    }
}