using System;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;

namespace ClassLibrary2
{
    public class Image
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Title { get; set; }
        public int Like { get; set; }

    }
    public class ImageManager
    {
       
            private string _connectionString;

            public ImageManager(string connectionString)
            {
                _connectionString = connectionString;
            }

            public IEnumerable<Image> GetImage()
            {
                using (var context = new ImageContext(_connectionString))
                {
                    return context.Image.ToList();
                }
            }

            public void Add(Image image)
            {
                using (var context = new ImageContext(_connectionString))
                {
                    context.Image.Add(image);
                    context.SaveChanges();
                }
            }

            public Image GetById(int id)
            {
                using (var context = new ImageContext(_connectionString))
                {
                    return context.Image.FirstOrDefault(p => p.Id == id);
                }
            }

            public void Update(Image image)
            {
                using (var context = new ImageContext(_connectionString))
                {
                    context.Image.Attach(image);
                    context.Entry(image).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }

            public void Delete(int id)
            {
                using (var context = new ImageContext(_connectionString))
                {
                    context.Database.ExecuteSqlCommand(
                        "DELETE FROM Image WHERE Id = @id",
                        new SqlParameter("@id", id));
                }
            }
        }
    public class ImageContext : DbContext
    {
        private string _connectionString;

        public ImageContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<Image> Image { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(_connectionString);
        }
    }
    public class ImageContextFactory : IDesignTimeDbContextFactory<ImageContext>
    {
        public ImageContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), $"..{Path.DirectorySeparatorChar}HW59_Instigram_April30"))
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true).Build();

            return new ImageContext(config.GetConnectionString("ConStr"));
        }
    }
}
