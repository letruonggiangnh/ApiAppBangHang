using ApiAppBangHang.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBanHang.Bussiness.UnitOfWork
{
    public class WorkUnit : IUnitOfWork, IDisposable
    {
        public AppBanSachDbContext Context { get; private set; }
        public WorkUnit(AppBanSachDbContext dbContext) 
        {
            Context = dbContext;
        }
        public void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
                Context = null;
            }
        }

        public void RollBack()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            if (Context != null)
            {
                Context.SaveChanges();
            }
        }
    }
}
