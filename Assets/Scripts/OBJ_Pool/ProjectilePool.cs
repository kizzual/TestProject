using System;
using System.Collections.Generic;
using UnityEngine;

namespace OBJ_pool
{
    public class ProjectilePool<T>
    {
        private readonly Func<T> m_preloadFunc;
        private readonly Action<T> m_getAction;
        private readonly Action<T> m_returnnAction;

        private Queue<T> m_pool = new Queue<T> ();
        private List<T> m_active = new List<T> ();

        public ProjectilePool(Func<T> preloadFunc, Action<T> getAction, Action<T> returnAction, int preloadCount)
        {

            m_preloadFunc = preloadFunc;
            m_getAction = getAction;
            m_returnnAction = returnAction;

            if(preloadFunc == null)
            {
                Debug.LogError("Preload function is null");
            }

            for (int i = 0; i < preloadCount; i++)
                Return(preloadFunc());
        }

        public T Get()
        {
            T item = m_pool.Count > 0 ? m_pool.Dequeue() : m_preloadFunc();
            m_getAction(item);
            m_active.Add(item);
            return item;
        }

        public void Return(T item)
        {
            m_returnnAction(item);
            m_pool.Enqueue(item);
            m_active.Remove(item);
        }

        public void ReturnAll()
        {
            foreach (T item in m_active.ToArray())
                Return(item);
        }

    }
}
