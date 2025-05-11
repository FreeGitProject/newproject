namespace TaskManagementAPI.Models
{
    public class TaskComment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }

        public int TaskId { get; set; }
        public Task Task { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
