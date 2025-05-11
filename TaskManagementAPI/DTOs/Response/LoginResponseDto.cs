namespace TaskManagementAPI.DTOs.Response
{
    // DTOs/Response/LoginResponseDto.cs
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string Role { get; set; }
    }
}
