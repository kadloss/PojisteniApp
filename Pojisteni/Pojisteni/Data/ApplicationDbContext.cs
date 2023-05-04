using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pojisteni.Models;

namespace Pojisteni.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Pojisteni.Models.Person> Person { get; set; } = default!;
        public DbSet<Pojisteni.Models.Insurance> Insurance { get; set; } = default!;
        public DbSet<Pojisteni.Models.InsuranceName> InsuranceName { get; set; } = default!;
    }
}