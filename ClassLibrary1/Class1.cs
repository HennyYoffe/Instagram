using System;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ClassLibrary1
{
    public class Image
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Title { get; set; }
        public int Like { get; set; }
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
//Create an application where people can upload images for others
//to see and "Like". 

//there should be a page on the site where a user can upload new images. When they do,
//they should also add a title for that image.

//On the home page, display a list of all images, sorted by most recent. With each
//image, display the title of that image. The image and title should be links, that
//when clicked, should take the user to a page where they see that individual
//image in large.

//Beneath that image, there should be a button that says "Like". When a user clicks 
//on this button, via ajax, update the likes count in the database. Once a user
//has liked an image, they should not be able to like it again (use cookies/session 
//for this). 

//Next to the Like button, there should be a number that displays the current amount
//of likes for this image. This number should be updated in real time, e.g. if someone
//else on a different machine likes this image, the number should update on my screen
//as well without me having to hit refresh. The way to do this last part is by using
//setInterval. In setInterval, make an ajax call to the server to get the current count
//of likes, and update the page with that number.