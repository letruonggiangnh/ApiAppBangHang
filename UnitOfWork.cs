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
