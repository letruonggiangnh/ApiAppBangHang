using ApiAppBangHang.Models;
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
        private bool disposed = false;
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
