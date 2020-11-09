﻿using System.Collections.Generic;

namespace Service.Queue.API.Services
{
    public interface IServiceList<T>
    {
        void Add(T item);
        int Count();
        List<T> GetItems();
    }

    public class ServiceList<T> : IServiceList<T> where T: class, new()
    {
        private readonly List<T> _listItems;

        public ServiceList()
        {
            _listItems = new List<T>();
        }

        public void Add(T item)
        {
            _listItems.Add(item);
        }

        public int Count()
        {
            return _listItems.Count;
        }

        public List<T> GetItems()
        {
            return _listItems;
        }
    }
}
