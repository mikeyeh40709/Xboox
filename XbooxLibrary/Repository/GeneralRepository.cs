﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XbooxLibrary.Models.DataTable;

namespace XbooxLibrary.Repository
{
    public class GeneralRepository<T> where T :class
    {
        private static XbooxLibraryDBContext context;
        protected XbooxLibraryDBContext Context
        {
            get { return context; }
        }

        public GeneralRepository(XbooxLibraryDBContext contexts)
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

        public void SaveContext()
        {
             context.SaveChanges();
        }

        public T GetFirst(Expression<Func<T, bool>> entity)
        {
            return context.Set<T>().FirstOrDefault<T>(entity); 
        }

    }
}
