using ApiAppBangHang.Models;
using AppBanHang.Bussiness.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBanHang.Bussiness.Repository
{
    public abstract class BaseRepository<T> : IDisposable
    {
        protected WorkUnit unit = null;
        private bool isWorkUnitGranular = true;
        public AppBanSachDbContext Context { get { return unit.Context; } }
        public BaseRepository(WorkUnit unit)
        {
            this.unit = unit;
            isWorkUnitGranular = false;
        }
        public void Dispose()
        {
            if (isWorkUnitGranular)
            {
                unit.Context.SaveChanges();
            }
            unit.Dispose();
        }
        public abstract object Create(T entity, ref string strError);
        //public abstract object ImportList(List<T> entity, ref string strError);
        public abstract object Update(T entity, ref string strError);
        public abstract string Delete(object entityId, ref string strError);
        public abstract T GetById(object entityId);
        public abstract List<T> List();
        public abstract List<T> ListByQuery(string strQuery);
        public abstract List<T> GetTableListById(object entityId);
    }
}
