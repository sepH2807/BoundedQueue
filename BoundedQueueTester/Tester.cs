using ReactiveUI;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BoundedQueueTester
{
    public class Tester
    {
        [Fact]
        public void TestAddingMainThread()
        {
            var service = new BoundedQueue.BoundedQueue<int>(size: 12);

            service.WhenAnyValue(x => x.observableCollection.Count).Subscribe(queueChange =>
            {
                System.Diagnostics.Debug.WriteLine($"ReadOnlyObservable changed:\n" +
                    $"Count: {queueChange}");
            });

            Task addingTask = Task.Run(() =>
            {
                service.Enqueue(1);

                Assert.True(service.Count == 1);

                service.Enqueue(2);

                Assert.True(service.Count == 2);

                var DeQueuedItem = service.Dequeue();

                Assert.True(DeQueuedItem.resultValue == 1);
                Assert.True(service.Count == 1);

                DeQueuedItem = service.Dequeue();

                Assert.True(DeQueuedItem.resultValue == 2);
                Assert.True(service.Count == 0);
            });

        }

        [Fact]
        public void TestAddingAndRemovingMultipleThreads()
        {
            var service = new BoundedQueue.BoundedQueue<int>(size: 128);

            Task.Factory.StartNew(() =>
            {
                int itemsToAdd = 40;

                Random r = new Random();

                for (int count = 0; count < itemsToAdd; count++)
                {
                    service.Enqueue(count);
                    System.Diagnostics.Debug.WriteLine($"Added {count} to Queue");
                    int waitTime = r.Next(2000) * 70;
                    Task.Delay(waitTime);
                }

            });

            Task.Factory.StartNew(() =>
            {
                Task.Delay(50).Wait();
                Random r = new Random();

                do
                {
                    //int waitTime = r.Next(500);
                    //Task.Delay(waitTime);
                    var DeQueued = service.Dequeue();
                    if (DeQueued.resultState == false)
                    {
                        System.Diagnostics.Debug.WriteLine($"Could not dequeue no items available");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Dequeued {DeQueued.resultValue}"); //--- Waittime: {waitTime}");
                    }
                } while (service.Count > 0);

            });
        }
    }
}
