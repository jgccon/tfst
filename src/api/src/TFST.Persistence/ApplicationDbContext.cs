using Microsoft.EntityFrameworkCore;

namespace TFST.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
}
