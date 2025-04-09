using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 1ro comando Add-Migration AddXXXXXX
//Add-Migration InitialMigration -Context AppDbContext(primera migracion, "Se crea" la Base de datos con el ORM)
// 2do comando Update-Database (se ejecuta todo lo que hay en la carpeta migration)

//este archivo son las instrucciones de mapeo del ORM...
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



        //---Fuent API---
        // Este método (OnModelCreating) se usa para definir cómo se deben mapear las entidades del dominio y pasarlo tablas de la base de datos
        // y establecer sus relaciones, conversiones (el enum a cadena) y datos iniciales (Seed Data).
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

            //para que los enums sean str en la bd y no 0, 1, 2...
            modelBuilder.Entity<User>()
                        .Property(u => u.Type)
                        .HasConversion(new EnumToStringConverter<UserType>());
            modelBuilder.Entity<User>()
                        .Property(u => u.Status)
                        .HasConversion(new EnumToStringConverter<Status>());

            modelBuilder.Entity<Project>()
                        .Property(p => p.Status)
                        .HasConversion(new EnumToStringConverter<Status>());

            modelBuilder.Entity<Tool>()
                        .Property(t => t.Status)
                        .HasConversion(new EnumToStringConverter<Status>());

            modelBuilder.Entity<UserProject>()
                        .Property(up => up.Status)
                        .HasConversion(new EnumToStringConverter<Status>());

        }
    }
}

