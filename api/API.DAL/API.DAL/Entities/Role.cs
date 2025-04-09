namespace API.DAL.Entities;

public class Role : IdentityRole
{
    public Role(string roleName) : base(roleName) {}
    public Role() { }
}