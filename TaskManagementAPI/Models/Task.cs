namespace TaskManagementAPI.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<TaskComment> Comments { get; set; }
    }
}
