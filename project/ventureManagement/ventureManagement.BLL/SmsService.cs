using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VentureManagement.DAL;
using VentureManagement.IBLL;
using VentureManagement.IDAL;
using VentureManagement.Models;

namespace VentureManagement.BLL
{
    public class SmsService : BaseService<Sms>, InterfaceSmsService
    {
        public SmsService() : base(RepositoryFactory.SmsRepository)
        {
        }

        public Sms Find(Expression<Func<Sms, bool>> whereLamdba) { return CurrentRepository.Find(whereLamdba); }
        public IQueryable<Sms> FindList(Expression<Func<Sms, bool>> whereLamdba)
        {
            return CurrentRepository.FindList(whereLamdba, "SmsId", false);
        }
    }
}
