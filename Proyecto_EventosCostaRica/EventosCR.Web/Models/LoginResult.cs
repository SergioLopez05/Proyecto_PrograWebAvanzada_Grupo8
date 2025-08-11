namespace EventosCR.Web.Models
{
    public class LoginResult
    {
        public string Token { get; set; } = "";
        public string? Role { get; set; }
        public string? UserName { get; set; }
        public string? UserId { get; set; }
    }

}
