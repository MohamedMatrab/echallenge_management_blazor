namespace API.DAL.Entities;

public class User : IdentityUser
{
    public string FirstName { get; set; } = "fn";
    public string LastName { get; set; } = "ln";
}