using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Domain.Identity;

namespace ProEventos.Infra.Context {
    public class InfraContext : IdentityDbContext<User, Role, int,
                                IdentityUserClaim<int>, 
                                UserRole, 
                                IdentityUserLogin<int>,
                                IdentityRoleClaim<int>, 
                                IdentityUserToken<int>>

    {
        public InfraContext(DbContextOptions<InfraContext> options):base(options)
        {
            
        }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Lote> Lotes { get; set; }
        public DbSet<Palestrante> Palestrantes { get; set; }
        public DbSet<PalestranteEvento> PalestrantesEventos { get; set; }
        public DbSet<RedeSocial> RedesSociais  { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(InfraContext).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.Restrict;
            
            //se isso nao for feito o identity nao funcionará
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<UserRole>(userRole => 
            {
                userRole.HasKey(ur => new {ur.UserId, ur.RoleId});

                userRole.HasOne(ur => ur.Role)
                        .WithMany(r => r.UserRoles)
                        .HasForeignKey(ur => ur.RoleId)
                        .IsRequired();

                userRole.HasOne(ur => ur.User)
                        .WithMany(r => r.UserRoles)
                        .HasForeignKey(ur => ur.UserId)
                        .IsRequired();
            }); 

            modelBuilder.Entity<PalestranteEvento>()
                .HasKey(PE => new {PE.EventoId, PE.PalestranteId});

            modelBuilder.Entity<Evento>()
                .HasMany(e => e.RedesSociais)
                .WithOne(rs => rs.Evento)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Palestrante>()
                .HasMany(e => e.RedesSociais)
                .WithOne(rs => rs.Palestrante)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}