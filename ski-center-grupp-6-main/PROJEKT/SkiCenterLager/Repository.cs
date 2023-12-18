using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SkiCenterLager
{
    public class Repository<T>
        where T : class
    {
        internal DbContext context;
        internal DbSet<T> table;


        public Repository(DbContext context)
        {
            this.context = context;
            table = context.Set<T>();
        }
        /// <summary>
        ///  Lägg till en ny entitet till tabell
        /// </summary>
        /// <param name="entity"></param>
        public void Add(T entity)
        {
            table.Add(entity);
        }

        /// <summary>
        ///  Hitta uppsättning entiteter som matchar ett villkor
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return table.Where(predicate);
        }
        /// <summary>
        ///  Hitta första entiteten som matchar ett villkor
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual T FirstOrDefault(Func<T, bool> predicate)
        {
            return table.FirstOrDefault(predicate);
        }
        public virtual IEnumerable<T> Query(Func<IQueryable<T>, IQueryable<T>> query) => query(table);

        public T FirstOrDefault(Func<T, bool> predicate, params Expression<Func<T, object>>[] includes)
        {
            var query = table.AsQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query.FirstOrDefault(predicate);
        }
        /// <summary>
        /// Uppdaterar tabell
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(T entity) => table.Update(entity);
        public virtual T Update(T oldEntity, T newEntity)
        {
            context.Entry(oldEntity).CurrentValues.SetValues(newEntity);
            table.Update(oldEntity);
            return oldEntity;
        }
        public virtual void Delete(int id)
        {
            var entity = table.Find(id);
            if (entity != null)
                context.Entry(entity).State = EntityState.Deleted;
        }
        public virtual T Delete(T entity) { table.Remove(entity); return entity; }
        public virtual void DeleteRange(IEnumerable<T> entities) => table.RemoveRange(entities);
    }
}
