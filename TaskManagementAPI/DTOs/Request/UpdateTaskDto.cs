using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.DTOs.Request
{
    public class UpdateTaskDto
    {
        [StringLength(100)]
        public string? Title { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }
        public bool? IsCompleted { get; set; }
    }
}
