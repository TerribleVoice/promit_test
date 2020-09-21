using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace promit_test
{
    class GlossaryDbContext : DbContext
    {
        public GlossaryDbContext() : base("DbConnection")
        { }

        public DbSet<Word> Words { get; set; }

        public void CreateGlossary()
        {
            ClearGlossary();
            UpdateGlossary();
        }

        public void UpdateGlossary()
        {
            var dbWordsSet = Words.ToHashSet();
            var founded = new List<Word>();
            var document = Document.ReadDocument();
            
            foreach (var word in document)
            {
                if (dbWordsSet.Contains(word))
                {
                    var get = dbWordsSet.First(el => el.Value == word.Value);
                    get.Amount += word.Amount;
                    founded.Add(word);
                }
            }

            Words.AddRange(document.Except(founded));
            SaveChanges();
        }

        public void ClearGlossary()
        {
            Console.Write("Clearing glossary");
            Database.ExecuteSqlCommand("TRUNCATE TABLE Words");
            Console.Write("\r \b");
        }
    }
}
