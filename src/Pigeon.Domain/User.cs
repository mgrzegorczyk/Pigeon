namespace Pigeon.Domain;

public class User
{
    public User(string username, string password, string salt)
    {
        Id = Guid.NewGuid();
        Username = username;
        Password = password;
        CreatedAt = DateTime.Now;
        Salt = salt;
    }
    
    public Guid Id { get; set; }
    public string Username {get; set; }
    public string Password { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Salt { get; set; }
}