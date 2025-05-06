using System.ComponentModel.DataAnnotations;
namespace TodoApplicationTemp.Models
{
    public class TodoItem
    {
        public int Id { get; set; }

        [Required]
        public required string Title { get; set; }

        public bool IsComplete { get; set; }

        [Display(Name = "Category")]
        public required string Category { get; set; }

        [Range(1, 5)]
        public int Priority { get; set; } // 1 Low Priority, 5 High Priority

        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime? DueDate { get; set; }

    }
}
