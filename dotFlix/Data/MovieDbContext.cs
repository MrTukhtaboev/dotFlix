using dotFlix.Models;
using Microsoft.EntityFrameworkCore;

namespace dotFlix.Data
{
    public class MovieDbContext : DbContext
    {
        public MovieDbContext(DbContextOptions<MovieDbContext> options)
            :base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Author> Authors { get; set; }
    }
}
