public class Bug
{
    public int Id { get; set; }
    public string Summary { get; set; }
    public string Description { get; set; }
    public bool Resolved { get; set; }

    public int UserId { get; set; }
    public User CreatedBy { get; set; }
}