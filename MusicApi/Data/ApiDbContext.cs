using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicApi.Models;

namespace MusicApi.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext>options) :base(options)
        {

        }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Album> Albums { get; set; }
                      
        public DbSet<Song> Songs { get; set; }
        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     modelBuilder.Entity<Song>().HasData(
        //         new Song
        //         {
        //             Id = 1,
        //             Title = "willow",
        //             Language = "English",
        //             Duration = "4.35"
        //         },
        //                         new Song
        //         {
        //             Id = 2,
        //             Title = "Despacico",
        //             Language = "Spanish",
        //             Duration = "4.35"
        //         }
             
        //     );
        // }


    }
}
