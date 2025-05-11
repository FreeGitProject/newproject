using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.DTOs.Request
{
    public class CreateTaskDto
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public DateTime? DueDate { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }
    }
}
