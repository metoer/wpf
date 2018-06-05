using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;

namespace Hytera.EEMS.Common
{
    /// <summary>
    /// 多线程List集合访问
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ThreadSafeObservable<T> : ObservableCollection<T>
    {
        public new int Count
        {
            get
            {
                lock (((ICollection)this).SyncRoot)
                {
                    return base.Count;
                }
            }
        }

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(T item)
        {
            lock (((ICollection)this).SyncRoot)
            {
                this.Add(item);
            }
        }

        /// <summary>
        /// 移除对象
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(T item)
        {
            lock (((ICollection)this).SyncRoot)
            {
                this.Remove(item);
            }
        }

        /// <summary>
        /// 寻找对象
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T Find(Func<T, bool> predicate)
        {
            lock (((ICollection)this).SyncRoot)
            {
                try
                {
                    if (this.Count == 0)
                    {
                        return default(T);
                    }

                    return this.SingleOrDefault(predicate);
                }
                catch
                {
                    return default(T);
                }
            }
        }

        /// <summary>
        /// 移除key对象
        /// </summary>
        /// <param name="predicate"></param>
        public void RemoveItemByKey(Func<T, bool> predicate)
        {
            lock (((ICollection)this).SyncRoot)
            {
                var objectData = this.Find(predicate);
                if (objectData != null)
                    this.Remove(objectData);
            }
        }

    }
}
