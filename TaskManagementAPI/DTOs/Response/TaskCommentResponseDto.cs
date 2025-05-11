namespace TaskManagementAPI.DTOs.Response
{

    public class TaskCommentResponseDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int TaskId { get; set; }

    }
}