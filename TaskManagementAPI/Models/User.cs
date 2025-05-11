namespace TaskManagementAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // In real app, store hashed passwords
        public string Role { get; set; } // "Admin" or "User"
        public ICollection<Task> Tasks { get; set; }
        public ICollection<TaskComment> Comments { get; set; }
    }
}
