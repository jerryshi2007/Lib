using System;
using System.Runtime.InteropServices;
using System.Data;
using System.Collections.Generic;

namespace Lib.Core
{
    /// <summary>
    /// 数据对象集合类的虚基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    [ComVisible(true)]
    public class DataObjectCollection<T> : ReadOnlyDataObjectCollection<T> 
    {
        public T this[int index]
        {
            get
            {
                return (T)this.List[index];
            }
            set
            {
                this.List[index] = value;
            }
        }

        public virtual void Add(T data)
        {
            if (data == null)
                return;

            InnerAdd(data);
        }


        public virtual void AddRange(IEnumerable<T> datalist)
        {
            if (datalist == null)
                return;

            foreach (T data in datalist)
            {
                InnerAdd(data);
            }
        }

        /// <summary>
        /// 从别的集合中复制(添加到现有的集合中)
        /// </summary>
        /// <param name="data"></param>
        public void CopyFrom(IEnumerable<T> data)
        {
            ExceptionHelper.FalseThrow<ArgumentNullException>(data != null, "data");

            IEnumerator<T> enumerator = data.GetEnumerator();

            while (enumerator.MoveNext())
                InnerAdd(enumerator.Current);
        }

        /// <summary>
        /// 删除满足条件的记录
        /// </summary>
        /// <param name="match"></param>
        public void Remove(Predicate<T> match)
        {
            int i = 0;

            while (i < this.Count)
            {
                T data = (T)List[i];

                if (match(data))
                    this.RemoveAt(i);
                else
                    i++;
            }
        }
    }
}
