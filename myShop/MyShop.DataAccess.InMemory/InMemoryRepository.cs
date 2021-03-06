﻿using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
    // note to self: this is how you use generic class templates in C#
    // the 'where' keyword is how you constrain the template T
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        ObjectCache cache = MemoryCache.Default;
        // list of this generic type
        List<T> items;
        string className;

        public InMemoryRepository()
        {
            className = typeof(T).Name; // this finds the class name for classes that inherit from it
            items = cache[className] as List<T>;
            if (items is null) items = new List<T>();
        }

        public void Commit() { cache[className] = items; }

        public void Insert(T t) { items.Add(t); }

        public void Update(T t)
        {
            T tToUpdate = items.Find(i => i.Id == t.Id);
            if (tToUpdate == null) // apparently the 'is' keyword doesn't work with generics
                throw new Exception(className + " not found");
            tToUpdate = t;
        }

        public T Find(string Id)
        {
            T t = items.Find(i => i.Id == Id);
            if (t == null) // apparently the 'is' keyword doesn't work with generics
                throw new Exception(className + " not found");
            return t;
        }

        public IQueryable<T> Collection() { return items.AsQueryable(); }

        public void Delete(string Id)
        {
            T tToDelete = items.Find(i => i.Id == Id);
            if (tToDelete == null) // apparently the 'is' keyword doesn't work with generics
                throw new Exception(className + " not found");
            items.Remove(tToDelete);
        }
    }
}
