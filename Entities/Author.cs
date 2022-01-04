namespace Dom;

public class Author : Entity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string PasswordHash { get; set; }
    public DateOnly SignUpDate { get; set; }
}
