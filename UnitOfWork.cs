﻿using ApiAppBangHang.Models;
using ApiAppBanSach.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace ApiAppBangHang
{
    public class UnitOfWork : IDisposable
    {
        public UnitOfWork(AppBanSachDbContext appBanSachDbContext) 
        {
            context = appBanSachDbContext;
        }
        AppBanSachDbContext context;
        private GenericRepository<AppUser> appUserRepository;
        private GenericRepository<ProductBook> productBookRepository;
        private GenericRepository<BookCategoryChild> bookCategoryChildRepository;
        private GenericRepository<BookDescription> bookDescriptionRepository;
        private GenericRepository<CartItem> cartItemRepository;
        private GenericRepository<Cart> cartRepository;

        private bool disposed = false;
        public GenericRepository<Cart> CartRepository
        {
            get
            {

                if (this.cartRepository == null)
                {
                    this.cartRepository = new GenericRepository<Cart>(context);
                }
                return cartRepository;
            }
        }
        public GenericRepository<CartItem> CartItemRepository
        {
            get
            {

                if (this.cartItemRepository == null)
                {
                    this.cartItemRepository = new GenericRepository<CartItem>(context);
                }
                return cartItemRepository;
            }
        }
        public GenericRepository<AppUser> AppUserRepository
        {
            get
            {

                if (this.appUserRepository == null)
                {
                    this.appUserRepository = new GenericRepository<AppUser>(context);
                }
                return appUserRepository;
            }
        }
        public GenericRepository<BookCategoryChild> BookCategoryChildRepository
        {
            get
            {
                if (this.bookCategoryChildRepository == null)
                {
                    this.bookCategoryChildRepository = new GenericRepository<BookCategoryChild>(context);
                }
                return bookCategoryChildRepository;
            }
        }

        public GenericRepository<ProductBook> ProductBookRepository
        {
            get 
            {
                if (this.productBookRepository == null) 
                {
                    this.productBookRepository = new GenericRepository<ProductBook>(context);
                }
                return productBookRepository;
            }
        }

        public GenericRepository<BookDescription> BookDescriptionRepository
        {
            get
            {
                if (this.bookDescriptionRepository == null)
                {
                    this.bookDescriptionRepository = new GenericRepository<BookDescription>(context);
                }
                return bookDescriptionRepository;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void Save()
        {
            context.SaveChanges();
        }
    }
}
