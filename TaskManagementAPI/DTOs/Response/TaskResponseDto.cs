namespace TaskManagementAPI.DTOs.Response
{
    public class TaskResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public ICollection<CommentResponseDto> Comments { get; set; }
    }
}
