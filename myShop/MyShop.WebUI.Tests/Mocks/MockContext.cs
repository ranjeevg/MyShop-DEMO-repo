using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.WebUI.Tests.Mocks
{
    public class MockContext<T> : IRepository<T> where T : BaseEntity
    {
        // list of this generic type
        List<T> items;
        string className;

        public MockContext()
        {
            items = new List<T>();
        }

        public void Commit() { return; }

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
