using ApiAppBanSach.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ApiAppBangHang.Models
{
    public class AppBanSachDbContext : IdentityDbContext<IdentityUser>
    {
        public AppBanSachDbContext()
        {
        }

        public AppBanSachDbContext(DbContextOptions options) : base(options)
        {
        }
       
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<AddressDetail> AddressDetails { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<ProductBook> ProductBooks { get; set; }
        public DbSet<BookCategoryChild> BookCategoryChilds { get; set; }
        public DbSet<BookCategoryParent> BookCategoryParents { get; set; }
        public DbSet<BookTag> BookTags { get; set; }
        public DbSet<BookDescription> BookDescriptions { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Cart> Carts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>(entity => {
                entity.HasKey(p => p.AppUserId);
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
            builder.Entity<ProductBook>(entity => {
                entity.HasKey(p => p.BookId);
                entity.HasOne(productBook => productBook.BookCategoryChild)
                       .WithMany(categoryChild => categoryChild.ProductBooks)
                       .HasForeignKey(productBook => productBook.CategoryChildId);

            });
            builder.Entity<ProductBook>(entity => {
                entity.HasOne(productBook => productBook.BookTag)
                       .WithMany(bookTag => bookTag.ProductBooks)
                       .HasForeignKey(productBook => productBook.TagId);
            });
            builder.Entity<BookCategoryChild>(entity => { 
                entity.HasKey(p => p.CategoryChildId);
                entity.HasOne(categoryChild => categoryChild.CategoryParent)
                      .WithMany(categoryParent => categoryParent.BookCategoryChilds)
                      .HasForeignKey(categoryChild => categoryChild.CategoryParentId);
            });
            builder.Entity<BookTag>(entity => {
                entity.HasKey(p => p.BookTagId);
            });
            builder.Entity<BookCategoryParent>(entity => {
                entity.HasKey(p => p.CategoryParentId);
            });
            builder.Entity<BookDescription>(entity => { 
                entity.HasKey(p => p.BookDescriptionId);
                entity.HasOne(bookDescription => bookDescription.ProductBook)
                      .WithMany(productBook => productBook.BookDescriptions)
                      .HasForeignKey(bookDescription => bookDescription.BookId);
            });
            builder.Entity<Cart>(entity =>
            {
                entity.HasKey(p => p.CartId);
                entity.HasOne(cart => cart.AppUser)
                      .WithOne(appUser => appUser.Cart)
                      .HasForeignKey<Cart>(cart => cart.UserId);
            });
            builder.Entity<CartItem>(entity =>
            {
                entity.HasKey(p => p.CartItemId);
                entity.HasOne(cartItem => cartItem.Cart)
                      .WithMany(cart => cart.CartItems)
                      .HasForeignKey(cartItem => cartItem.CartId);
                entity.HasOne(cartItem => cartItem.ProductBook)
                      .WithOne(productBook => productBook.CartItem)
                      .HasForeignKey<CartItem>(cartitem => cartitem.BookId);
            });
        }
    }
}
