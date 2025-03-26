using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Entities
{
    public class ExempleApiDbContext : IdentityDbContext<Account, Role, int, IdentityUserClaim<int>, AccountRoles, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public ExempleApiDbContext(DbContextOptions<ExempleApiDbContext> options)
           : base(options) { }

        public virtual DbSet<SystemSession> SystemSessions { get; set; }
        public virtual DbSet<BlockBruteForce> BlockBruteForces { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<BookLoan> BookLoans { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountRoles> AccountRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // modelBuilder.HasPostgresExtension("uuid-ossp");
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new AccountsConfiguration());
            // modelBuilder.HasDefaultSchema("pims");

            #region CustomEntitysIdentity
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("accounts");

                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.AccessFailedCount).HasColumnName("access_failed_count");
                entity.Property(e => e.Active).HasColumnName("active");
                entity.Property(e => e.ConcurrencyStamp).HasColumnName("concurrency_stamp");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.CreatedBy).HasMaxLength(255).HasColumnName("created_by");
                entity.Property(e => e.Email).HasMaxLength(255).HasColumnName("email");
                entity.Property(e => e.FirstName).HasMaxLength(255).HasColumnName("first_name");
                entity
                    .Property(e => e.Language)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("language");
                entity.Property(e => e.LastName).HasMaxLength(255).HasColumnName("last_name");
                entity.Property(e => e.LockoutEnabled).HasColumnName("lockout_enabled");
                entity.Property(e => e.LockoutEnd).HasColumnName("lockout_end");
                entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
                entity.Property(e => e.NormalizedEmail).HasColumnName("normalized_email");
                entity.Property(e => e.EmailConfirmed).HasColumnName("email_confirmed");
                entity.Property(e => e.NormalizedUserName).HasColumnName("normalized_username");
                entity.Property(e => e.PhoneNumber).HasMaxLength(20).HasColumnName("phonenumber");
                entity.Property(e => e.SecurityStamp).HasColumnName("security_stamp");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                entity.Property(e => e.UpdatedBy).HasMaxLength(255).HasColumnName("updated_by");
                entity.Property(e => e.UserName).HasMaxLength(255).HasColumnName("username");
                entity.Property(e => e.PhoneNumberConfirmed).HasColumnName("phonenumber_confirmed");
                entity.Property(e => e.TwoFactorEnabled).HasColumnName("twofactor_enabled");
                entity.Ignore(e => e.PhoneNumberConfirmed);
                entity.Ignore(e => e.TwoFactorEnabled);
                // entity.Property(e => e.PhoneNumberConfirmed).HasColumnName("phonenumber_confirmed");
                // entity.Property(e => e.TwoFactorEnabled).HasColumnName("twofactor_enabled");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");

                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex").IsUnique();

                entity.Property(r => r.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.CreatedBy).HasColumnName("created_by");
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(255);
                entity
                    .Property(e => e.RoleCode)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValueSql("''")
                    .HasColumnName("role_code");
                entity
                    .Property(e => e.NormalizedName)
                    .HasColumnName("normalized_name")
                    .HasMaxLength(255);
                entity.Property(e => e.ConcurrencyStamp).HasColumnName("concurrency_stamp");
                entity.Property(e => e.Active).HasColumnName("active");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });

            modelBuilder.Entity<AccountRoles>(entity =>
            {
                entity.ToTable("account_roles");

                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId, "IX_Accounts_Roles_role_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.RoleId).HasColumnName("role_id");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.CreatedBy).HasColumnName("created_by");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
                entity
                    .HasOne(d => d.Role)
                    .WithMany(p => p.AccountRoles)
                    .HasForeignKey(d => d.RoleId);
                entity
                    .HasOne(d => d.Account)
                    .WithMany(p => p.AccountRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("account_claim");
            modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("role_claim");
            modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("account_login");
            modelBuilder.Entity<IdentityUserToken<int>>().ToTable("account_token");

            // modelBuilder.Ignore<IdentityUserClaim<int>>();
            // modelBuilder.Ignore<IdentityRoleClaim<int>>();
            // modelBuilder.Ignore<IdentityUserLogin<int>>();
            #endregion

            modelBuilder.Entity<SystemSession>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("system_session_pk");

                entity.ToTable("system_sessions");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.AccountId).HasColumnName("account_id");
                entity.Property(e => e.Browser)
                    .HasMaxLength(50)
                    .HasColumnName("browser");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.CreatedBy).HasColumnName("created_by");
                entity.Property(e => e.ExpirationTime).HasColumnName("expiration_time");
                entity.Property(e => e.IssuedTime).HasColumnName("issued_time");
                entity.Property(e => e.LoginAt).HasColumnName("login_at");
                entity.Property(e => e.LoginIp)
                    .HasMaxLength(50)
                    .HasColumnName("login_ip");
                entity.Property(e => e.Os)
                    .HasMaxLength(50)
                    .HasColumnName("os");
                entity.Property(e => e.Platform)
                    .HasMaxLength(50)
                    .HasColumnName("platform");
                entity.Property(e => e.RefreshTokenAt).HasColumnName("refresh_token_at");
                entity.Property(e => e.SessionStatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValue("A")
                    .HasComment("B (Blocked): Session ยังไม่ได้ใช้งาน\r\nA (Active): Session กำลังใช้งานอยู่\r\nE (Expired): Session หมดอายุแล้ว")
                    .HasColumnName("session_status");
                entity.Property(e => e.Token).HasColumnName("token");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });

            modelBuilder.Entity<BlockBruteForce>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("block_bruteforce_pk");

                entity.ToTable("block_bruteforce");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.Count).HasColumnName("count");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.CreatedBy).HasColumnName("created_by");
                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");
                entity.Property(e => e.LockedTime).HasColumnName("locked_time");
                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValue("A")
                    .HasComment("L (Locked): ถูกล็อก\r\nU (UnLock): ไม่ล็อก")
                    .HasColumnName("status");
                entity.Property(e => e.UnLockTime).HasColumnName("unlock_time");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("books_pkey");

                entity.ToTable("books");

                entity.HasIndex(e => e.Author, "idx_books_author");

                entity.HasIndex(e => e.Status, "idx_books_status");

                entity.HasIndex(e => e.Title, "idx_books_title");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Author)
                    .HasMaxLength(100)
                    .HasColumnName("author");
                entity.Property(e => e.Category)
                    .HasMaxLength(50)
                        .HasColumnName("category");
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnName("created_at");
                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(255)
                    .HasColumnName("created_by");
                entity.Property(e => e.Isbn)
                    .HasMaxLength(13)
                    .HasColumnName("isbn");
                entity.Property(e => e.PublicationYear).HasColumnName("publication_year");
                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("'Available'::character varying")
                    .HasColumnName("status");
                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .HasColumnName("title");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(255)
                    .HasColumnName("updated_by");
            });  

            modelBuilder.Entity<BookLoan>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("book_loans_pkey");

                entity.ToTable("book_loans");

                entity.HasIndex(e => e.AccountId, "idx_book_loans_account_id");

                entity.HasIndex(e => e.BookId, "idx_book_loans_book_id");

                entity.HasIndex(e => e.DueDate, "idx_book_loans_due_date");

                entity.HasIndex(e => e.Status, "idx_book_loans_status");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.AccountId).HasColumnName("account_id");
                entity.Property(e => e.BookId).HasColumnName("book_id");
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnName("created_at");
                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(255)
                    .HasColumnName("created_by");
                entity.Property(e => e.DueDate).HasColumnName("due_date");
                entity.Property(e => e.LoanDate)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnName("loan_date");
                entity.Property(e => e.ReturnDate).HasColumnName("return_date");
                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("'Active'::character varying")
                    .HasColumnName("status");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(255)
                    .HasColumnName("updated_by");

                entity.HasOne(d => d.Account).WithMany(p => p.BookLoans)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("fk_book_loans_accounts");

                entity.HasOne(d => d.Book).WithMany(p => p.BookLoans)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("fk_book_loans_books");
            });
        }
    }

    public class CMSDevDbContextFactory : IDesignTimeDbContextFactory<ExempleApiDbContext>
    {
        public ExempleApiDbContext CreateDbContext(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ExempleApiDbContext>();
            var conn = config.GetConnectionString("DefaultConnection");
            optionsBuilder.UseNpgsql(conn);
            return new ExempleApiDbContext(optionsBuilder.Options);
        }
    }
}
