using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBanHang.Bussiness.UnitOfWork
{
    public interface IUnitOfWork
    {
        void Save();
        void RollBack();
    }
}
