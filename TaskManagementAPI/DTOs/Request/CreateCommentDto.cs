using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.DTOs.Request
{
    public class CreateCommentDto
    {
        [Required]
        [StringLength(1000)]
        public string Content { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int TaskId { get; set; }
    }
}
