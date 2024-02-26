using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiAppBanSach.Models;
using AppBanHang.Bussiness.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace AppBanHang.Bussiness.Repository
{
    public class UserRepository : BaseRepository<AppUser>
    {
        public UserRepository(WorkUnit unit) : base(unit)
        {
        }

        public override object Create(AppUser entity, ref string strError)
        {
            try
            {
                Context.Entry(entity).State = EntityState.Added;
                Context.SaveChanges();
                return entity.AppUserId;
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return 0;
            }
        }

        public override string Delete(object entityId, ref string strError)
        {
            throw new NotImplementedException();
        }

        public override AppUser GetById(object entityId)
        {
            throw new NotImplementedException();
        }

        public override List<AppUser> GetTableListById(object entityId)
        {
            throw new NotImplementedException();
        }

        public override List<AppUser> List()
        {
            throw new NotImplementedException();
        }

        public override List<AppUser> ListByQuery(string strQuery)
        {
            throw new NotImplementedException();
        }

        public override object Update(AppUser entity, ref string strError)
        {
            throw new NotImplementedException();
        }
    }
}
