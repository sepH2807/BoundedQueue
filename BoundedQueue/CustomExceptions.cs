using System;
using System.Collections.Generic;
using System.Text;

namespace BoundedQueue
{
    public class QueueFullException : Exception
    {
        public QueueFullException()
        {

        }

        public QueueFullException(string message) : base(message)
        {

        }

        public QueueFullException(string message,Exception inner): base(message,inner)
        {

        }
    }

    public class QueueEmptyException : Exception
    {
        public QueueEmptyException()
        {

        }

        public QueueEmptyException(string message) : base(message)
        {

        }

        public QueueEmptyException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
