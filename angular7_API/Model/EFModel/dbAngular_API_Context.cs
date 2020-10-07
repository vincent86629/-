using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace angular_API.Model.EFModel
{
    public partial class dbAngular_API_Context : DbContext
    {
        public dbAngular_API_Context()
        {
        }

        public dbAngular_API_Context(DbContextOptions<dbAngular_API_Context> options)
            : base(options)
        {
        }

        public virtual DbSet<MapPermissionMenu> MapPermissionMenu { get; set; }
        public virtual DbSet<MapUserPermission> MapUserPermission { get; set; }
        public virtual DbSet<TblAdmin> TblAdmin { get; set; }
        public virtual DbSet<TblMenu> TblMenu { get; set; }
        public virtual DbSet<TblOperationLog> TblOperationLog { get; set; }
        public virtual DbSet<TblPermission> TblPermission { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=MSI\\SQLEXPRESS;Database=YangTingSecurity;user id=vincent;password=immie0983");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MapPermissionMenu>(entity =>
            {
                entity.HasKey(e => new { e.PermissionId, e.MenuId });

                entity.ToTable("map_PermissionMenu");

                entity.Property(e => e.PermissionId).HasColumnName("PermissionID");

                entity.Property(e => e.MenuId).HasColumnName("MenuID");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.MapPermissionMenu)
                    .HasForeignKey(d => d.MenuId)
                    .HasConstraintName("FK_map_PermissionMenu_tbl_Menu");

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.MapPermissionMenu)
                    .HasForeignKey(d => d.PermissionId)
                    .HasConstraintName("FK_map_PermissionMenu_tbl_Permission");
            });

            modelBuilder.Entity<MapUserPermission>(entity =>
            {
                entity.HasKey(e => new { e.AdminId, e.PermissionId });

                entity.ToTable("map_UserPermission");

                entity.Property(e => e.AdminId).HasColumnName("AdminID");

                entity.Property(e => e.PermissionId).HasColumnName("PermissionID");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.MapUserPermission)
                    .HasForeignKey(d => d.AdminId)
                    .HasConstraintName("FK_map_UserPermission_tbl_Admin");

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.MapUserPermission)
                    .HasForeignKey(d => d.PermissionId)
                    .HasConstraintName("FK_UserPermission_Permission");
            });

            modelBuilder.Entity<TblAdmin>(entity =>
            {
                entity.ToTable("tbl_Admin");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Account)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("EmployeeID")
                    .HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Phone).HasMaxLength(50);
            });

            modelBuilder.Entity<TblMenu>(entity =>
            {
                entity.ToTable("tbl_Menu");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.Url).HasColumnName("URL");
            });

            modelBuilder.Entity<TblOperationLog>(entity =>
            {
                entity.ToTable("tbl_OperationLog");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Ip)
                    .HasColumnName("IP")
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.RequestTime).HasColumnType("datetime");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.OperatorNavigation)
                    .WithMany(p => p.TblOperationLog)
                    .HasForeignKey(d => d.Operator)
                    .HasConstraintName("FK_OperationLog_Admin");
            });

            modelBuilder.Entity<TblPermission>(entity =>
            {
                entity.ToTable("tbl_Permission");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CodeName)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }
    }
}
