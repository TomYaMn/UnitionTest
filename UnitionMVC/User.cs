using System.Collections.Generic;
public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }

    public ICollection<Bug> Bugs { get; set; }
}