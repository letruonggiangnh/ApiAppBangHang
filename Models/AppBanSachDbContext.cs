using ApiAppBanSach.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ApiAppBangHang.Models
{
    public class AppBanSachDbContext : IdentityDbContext<IdentityUser>
    {
        public AppBanSachDbContext(DbContextOptions<AppBanSachDbContext> options) : base(options)
        {
        }
       
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<AddressDetail> AddressDetails { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>(entity => {
                entity.HasKey(p => p.Id);
                entity.HasMany(user => user.UserAddresses)
                    .WithOne(userAddress => userAddress.User)
                    .HasForeignKey(userAddress => userAddress.UserId);
            });

            builder.Entity<UserAddress>(entity =>
            {
                entity.HasKey(p => p.UserAddressId);
                entity.HasOne(userAddress => userAddress.AddressDetail)
                    .WithOne(addressDetail => addressDetail.UserAddress)
                    .HasForeignKey<AddressDetail>(addressDetail => addressDetail.UserId);
            });

            builder.Entity<AddressDetail>(entity =>
            {
                entity.HasKey(p => p.AdressDetailId);
            });
        }
    }
}
