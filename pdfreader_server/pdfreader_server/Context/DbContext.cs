
using Microsoft.EntityFrameworkCore;
using pdfreader_server.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace pdfreader_server
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().ToTable("users");
            builder.Entity<UserFileObj>().ToTable("files");
            builder.Entity<UserFileObj>().ToTable("PdfFiles");
            builder.Entity<UserFileObj>().ToTable("ExcelFiles");
        }
    }
}