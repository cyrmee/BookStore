using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configuration;

public class ModelBuilderConfiguration
{
    public static void Apply(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new BookConfiguration());
    }
}
