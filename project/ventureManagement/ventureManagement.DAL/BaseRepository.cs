using ventureManagement.IDAL;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ventureManagement.DAL
{
    /// <summary>
    /// 仓储基类
    /// <remarks>
    /// 创建：2014.02.03
    /// 修改：2014.02.16
    /// </remarks>
    /// </summary>
    public class BaseRepository<T> : InterfaceBaseRepository<T> where T : class
    {
        protected VentureManagementDbContext nContext = ContextFactory.GetCurrentContext();

        public T Add(T entity)
        {
            nContext.Set<T>().Add(entity);
            nContext.SaveChanges();
            return entity;
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return nContext.Set<T>().Count(predicate);
        }

        public bool Update(T entity)
        {
            nContext.Set<T>().Attach(entity);
            nContext.Entry<T>(entity).State = System.Data.Entity.EntityState.Modified;
            return nContext.SaveChanges() > 0;
        }

        public bool Delete(T entity)
        {
            nContext.Set<T>().Attach(entity);
            nContext.Entry<T>(entity).State = System.Data.Entity.EntityState.Deleted;
            return nContext.SaveChanges() > 0;
        }

        public bool Exist(Expression<Func<T, bool>> anyLambda)
        {
            return nContext.Set<T>().Any(anyLambda);
        }

        public T Find(Expression<Func<T, bool>> whereLambda)
        {
            T entity = nContext.Set<T>().FirstOrDefault<T>(whereLambda);
            return entity;
        }

        public IQueryable<T> FindList(Expression<Func<T, bool>> whereLamdba, string orderName, bool isAsc)
        {
            var list = nContext.Set<T>().Where(whereLamdba);
            list = OrderBy(list, orderName, isAsc);
            return list;
        }

        public IQueryable<T> FindPageList(int pageIndex, int pageSize, out int totalRecord, Expression<Func<T, bool>> whereLamdba, string orderName, bool isAsc)
        {
            var list = nContext.Set<T>().Where<T>(whereLamdba);
            totalRecord = list.Count();
            list = OrderBy(list, orderName, isAsc).Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            return list;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">原IQueryable</param>
        /// <param name="propertyName">排序属性名</param>
        /// <param name="isAsc">是否正序</param>
        /// <returns>排序后的IQueryable<T></returns>
        private IQueryable<T> OrderBy(IQueryable<T> source, string propertyName, bool isAsc)
        {
            if (source == null) throw new ArgumentNullException("source", "不能为空");
            if (string.IsNullOrEmpty(propertyName)) return source;
            var parameter = Expression.Parameter(source.ElementType);
            var property = Expression.Property(parameter, propertyName);
            if (property == null) throw new ArgumentNullException("propertyName", "属性不存在");
            var lambda = Expression.Lambda(property, parameter);
            var methodName = isAsc ? "OrderBy" : "OrderByDescending";
            var resultExpression = Expression.Call(typeof(Queryable), methodName, new Type[] { source.ElementType, property.Type }, source.Expression, Expression.Quote(lambda));
            return source.Provider.CreateQuery<T>(resultExpression);
        }
    }
}
