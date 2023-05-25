using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public class User : IdentityUser
{
    public IEnumerable<Order> Orders { get; } = new List<Order>();
}