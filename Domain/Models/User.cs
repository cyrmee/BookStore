using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public class User : IdentityUser
{
    public ICollection<Order> Orders { get; } = new List<Order>();
}