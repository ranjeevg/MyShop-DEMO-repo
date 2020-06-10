using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
    // note to self: this is how you use generic class templates in C#
    public class InMemoryRepository<T>
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
            T tToUpdate = items.Find(i => i.ID == t.ID);
            if (tToUpdate == null) // apparently the 'is' keyword doesn't work with generics
                throw new Exception(className + " not found");
            tToUpdate = t;
        }

        public T Find (string ID)
        {
            T t = items.Find(i => i.ID == ID);
            if (t == null) // apparently the 'is' keyword doesn't work with generics
                throw new Exception(className + " not found");
            return t;
        }

        public IQueryable<T> Collection() { return items.AsQueryable(); }

        public void Delete (string ID)
        {
            T tToDelete = items.Find(i => i.ID == ID);
            if (tToDelete == null) // apparently the 'is' keyword doesn't work with generics
                throw new Exception(className + " not found");
            items.Remove(tToDelete);
        }
    }
}
