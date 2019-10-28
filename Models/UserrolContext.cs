using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVC_users.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MVC_users.Models
{
    public class UserrolContext : DbContext
    {

         public UserrolContext()
        {
        }
         public UserrolContext(DbContextOptions<UserrolContext> options)
          : base(options)
        {
        }
          public  DbSet<Roles> Roless { get; set; }
          public  DbSet<UsuarioRol> UsuarioRoles { get; set; }
          public  DbSet<Usuarios> Usuarioss { get; set; }
          public  DbSet<Usuariosnomrer> Usuariosr { get; set; }
          protected override void  OnConfiguring(DbContextOptionsBuilder optionsBuilder)
          {
            optionsBuilder.UseNpgsql("Host=localhost;Database=apps;Username=postgres;Password=123");
          }
          protected override void OnModelCreating(ModelBuilder modelBuilder)
          {
            modelBuilder.HasDefaultSchema("public").Entity<Roles>().ToTable("roles")
            .HasKey(r => r.idroles);  

            modelBuilder.Entity<Usuarios>(entity =>
            {
                entity.ToTable("usuarios");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nombre)
                  .HasColumnName("nombre")
                  .HasMaxLength(45);

                entity.Property(e => e.Pass)
                  .HasColumnName("pass")
                  .HasMaxLength(45);

                entity.Property(e => e.Userid)
                  .HasColumnName("userid")
                  .HasMaxLength(45);          
             
            });
                
            modelBuilder.HasDefaultSchema("public").Entity<UsuarioRol>().ToTable("usuario_rol")
            .HasKey(r => new{r.idusuario, r.idrol});

            modelBuilder.Entity<UsuarioRol>() 
            .HasOne(pt => pt.Roles)
            .WithMany(p => p.UsuarioLink)
            .HasForeignKey(pt => pt.idrol);
        
            modelBuilder.Entity<UsuarioRol>() 
            .HasOne(pt => pt.Usuarios) 
            .WithMany(t => t.RolesLink)
            .HasForeignKey(pt => pt.idusuario);

          }          
    }

  public class Roles{
    public int idroles {get; set; }
    public string nombrerol {get; set; }
    public virtual ICollection<UsuarioRol> UsuarioLink { get; set; }
  }
  public  class UsuarioRol{        
    public int idur { get; set; }
    public int? idusuario { get; set; }
    public int? idrol { get; set; }

    public virtual Roles Roles { get; set; }
    public virtual Usuarios Usuarios { get; set; }
  }
 public  class Usuarios{
    public string Nombre { get; set; } 
    public string Pass { get; set; }
    public int Id { get; set; }
    public string Userid { get; set; }        
    
    public virtual ICollection<UsuarioRol> RolesLink { get; set; }
  }
    public  class Usuariosnomrer{
    public string Nombre { get; set; }
    public string Pass { get; set; }
    public int Id { get; set; }
    public string Userid { get; set; }
    public string nombrerol {get; set; } 
    }
}