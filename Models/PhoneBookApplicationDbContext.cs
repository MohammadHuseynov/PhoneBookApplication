using Microsoft.EntityFrameworkCore;
using PhoneBookApplication.Models.DomainModels.PersonAggregates;
using System.Globalization;
using System.Reflection.Emit;
using PhoneBookApplication.Models.DomainModels;

namespace PhoneBookApplication.Models
{
    public class PhoneBookApplicationDbContext : DbContext
    {
        public PhoneBookApplicationDbContext()
        {
            
        }

        public PhoneBookApplicationDbContext(DbContextOptions options) : base(options)
        {
           
        }

        public DbSet<Person> Person { get; set; }
        public DbSet<Image> Image { get; set; }
    }
}
