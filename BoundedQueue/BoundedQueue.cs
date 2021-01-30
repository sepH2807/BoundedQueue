using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;

namespace BoundedQueue
{
    public class BoundedQueue<T> : IBoundedQueue<T>
    {
        #region Deklarations
        private int _count { get; set; }
        public int Count
        {
            get
            {
                return _count;
            }
        }

        public int Size { get; private set; }

        private ObservableCollection<T> _queue = null;
        public ReadOnlyObservableCollection<T> observableCollection;
        #endregion
        public BoundedQueue(int size)
        {
            Size = size;
            _queue = new ObservableCollection<T>();
            RegisterReadonlyObservableCollection();
        }

        public void Enqueue(T element)
        {
            Monitor.Enter(_queue);
            //System.Diagnostics.Debug.WriteLine($"Enqueue from Thread: {Thread.CurrentThread.ManagedThreadId}");
            if (_queue.Count() > Size)
            {
                //The Queue is already full
                //block adding item to queue
                //inform that the item was not added
                throw new QueueFullException($"Queue is already completly filled exception.\nCount: {_queue.Count()}\nSize: {Size}");
            }

            //lock (_queueLock)
            //{
            _queue.Add(element);
            _count = _queue.Count();
            //}
            //Monitor.PulseAll(_queue);
            Monitor.Exit(_queue);
            //System.Diagnostics.Debug.WriteLine($"Exit Enqueue from Thread: {Thread.CurrentThread.ManagedThreadId}");
        }

        public (T resultValue, bool resultState) Dequeue()
        {
            T FirstItem = default;
            bool ResultState = true;
            Monitor.Enter(_queue);
            //System.Diagnostics.Debug.WriteLine($"Dequeue from Thread: {Thread.CurrentThread.ManagedThreadId}");
            if (_queue.Count() == 0)
            {
                //throw new QueueEmptyException($"Queue is already empty. Can not dequeue item.");
                ResultState = false;
            }

            if (ResultState == true)
            {
                FirstItem = _queue[0];
                _queue.RemoveAt(0);
                _count = _queue.Count();
            }


            //Monitor.PulseAll(_queue);
            Monitor.Exit(_queue);
            //System.Diagnostics.Debug.WriteLine($"Exit Dequeue from Thread: {Thread.CurrentThread.ManagedThreadId}");
            return (FirstItem, true);
        }

        private void RegisterReadonlyObservableCollection()
        {
            if (observableCollection == null)
            {
                _queue
                     .ToObservableChangeSet()
                     .AsObservableList()
                     .Connect()
                     .ObserveOn(RxApp.MainThreadScheduler)
                     .Bind(out observableCollection)
                     .DisposeMany()
                     .Subscribe();
            }
        }
    }
}
