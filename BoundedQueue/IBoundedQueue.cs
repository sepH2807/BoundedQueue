using System;
using System.Collections.Generic;
using System.Text;

namespace BoundedQueue
{
    public interface IBoundedQueue<T>
    {
        void Enqueue(T element);
        (T resultValue, bool resultState) Dequeue();
        int Count { get; }
        int Size { get; }
    }
}
