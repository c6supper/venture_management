using VentureManagement.IBLL;
using VentureManagement.IDAL;

namespace VentureManagement.BLL
{
    /// <summary>
    /// 服务基类
    /// <remarks>创建：2014.02.03</remarks>
    /// </summary>
    public abstract class BaseService<T> : InterfaceBaseService<T> where T : class
    {
        protected InterfaceBaseRepository<T> CurrentRepository { get; set; }

        protected BaseService(InterfaceBaseRepository<T> currentRepository) { CurrentRepository = currentRepository; }

        public T Add(T entity) { return CurrentRepository.Add(entity); }

        public bool Update(T entity) { return CurrentRepository.Update(entity); }

        public bool Delete(T entity) { return CurrentRepository.Delete(entity); }

        virtual public bool Initilization()
        {
            return true;
        }
    }
}
