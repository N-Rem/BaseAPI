using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//Son las instrucciones de mapeo del ORM
namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        //dbset recive una entidad y permite traducirlo a una tabla y la tabla con ese nombre se traduce a entidad
       public DbSet<User> Users { get; set; }
       public DbSet<Project> Projects { get; set; }
       public DbSet<UserProject> UserProjects { get; set; }
       public DbSet<Tool> Tools { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //relacion de las tools con el user
            modelBuilder.Entity<Tool>()
                .HasOne<User>() //Tool tiene una relación con User
                .WithMany()
                .HasForeignKey(t => t.UserTool);

            //relaciones entre user y prject
            modelBuilder.Entity<UserProject>()
                .HasOne<Project>()
                .WithMany()
                .HasForeignKey(p => p.ProjectId);
            //.OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserProject>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(p => p.UserId);
        }
    }
}

