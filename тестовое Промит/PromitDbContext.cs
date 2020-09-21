using System.Data.Entity;

namespace promit_test
{
    class GlossaryDbContext : DbContext
    {
        public GlossaryDbContext() : base("DbConnection")
        { }

        public DbSet<Word> Words { get; set; }
    }
}
