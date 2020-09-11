using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xboox.Models.DataTable;

namespace XbooxLibrary.Repository
{
    public class GeneralRepository<T> where T :class
    {
        private XbooxContext context;
        protected XbooxContext Context
        {
            get { return context; }
        }

        public GeneralRepository(XbooxContext contexts)
        {
            if (contexts == null)
            {
                throw new ArgumentNullException();
            }
            context = contexts;
        }

        public void Create(T entity)
        {
            context.Entry(entity).State = EntityState.Added;
        }

        public void Update(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }
        public void Delete(T entity)
        {
            context.Entry(entity).State = EntityState.Deleted;
        }

        public IQueryable<T> GetAll()
        {
            return context.Set<T>();
        }

    }
}
