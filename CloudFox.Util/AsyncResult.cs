using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading;

namespace CloudFox.Util
{
    public sealed class AsyncResult : IAsyncResult
    {
        private readonly AsyncCallback asyncCallback;
        private readonly object asyncState;

        private AutoResetEvent autoResetEvent;
        private Exception exception;

        private volatile bool isCompleted;

        public AsyncResult(AsyncCallback asyncCallback, object asyncState)
        {
            this.asyncCallback = asyncCallback;
            this.asyncState = asyncState;
        }

        public void SetAsCompleted(Exception exception, bool completedSynchronously)
        {
            lock (this)
            {
                this.exception = exception;
                CompletedSynchronously = completedSynchronously;
                isCompleted = true;

                if (autoResetEvent != null)
                    autoResetEvent.Set();
            }

            asyncCallback(this);
        }

        public void EndInvoke()
        {
            // This method assumes that only 1 thread calls EndInvoke 
            // for this object
            if (!IsCompleted)
            {
                // If the operation isn't done, wait for it
                AsyncWaitHandle.WaitOne();
                AsyncWaitHandle.Close();
                this.autoResetEvent = null;  // Allow early GC
            }

            // Operation is done: if an exception occured, throw it
            if (exception != null) 
                throw exception;
        }

        public object AsyncState
        {
            get 
            {
                return this.asyncState;
            }
        }

        public WaitHandle AsyncWaitHandle
        {
            get 
            {
                lock (this)
                {
                    if (this.autoResetEvent == null)
                        this.autoResetEvent = new AutoResetEvent(false);

                    return this.autoResetEvent;
                }
            }
        }

        public bool CompletedSynchronously { get; private set; }

        public bool IsCompleted 
        {
            get
            {
                return isCompleted;
            }
        }
    }

    public sealed class AsyncResult<T> : IAsyncResult
    {
        private readonly AsyncCallback asyncCallback;
        private readonly object asyncState;

        private AutoResetEvent autoResetEvent;
        private T result;
        private Exception exception;

        private volatile bool isCompleted;

        public AsyncResult(AsyncCallback asyncCallback, object asyncState)
        {
            this.asyncCallback = asyncCallback;
            this.asyncState = asyncState;
        }

        public void SetAsCompleted(T result, bool completedSynchronously)
        {
            lock (this)
            {
                this.result = result;
                CompletedSynchronously = completedSynchronously;
                isCompleted = true;

                if (autoResetEvent != null)
                    autoResetEvent.Set();
            }

            asyncCallback(this);
        }

        public void SetAsCompleted(Exception exception, bool completedSynchronously)
        {
            lock (this)
            {
                this.exception = exception;
                CompletedSynchronously = completedSynchronously;
                isCompleted = true;

                if (autoResetEvent != null)
                    autoResetEvent.Set();
            }

            asyncCallback(this);
        }

        public void EndInvoke()
        {
            // This method assumes that only 1 thread calls EndInvoke 
            // for this object
            if (!IsCompleted)
            {
                // If the operation isn't done, wait for it
                AsyncWaitHandle.WaitOne();
                AsyncWaitHandle.Close();
                this.autoResetEvent = null;  // Allow early GC
            }

            // Operation is done: if an exception occured, throw it
            if (exception != null)
                throw exception;
        }

        public object AsyncState
        {
            get
            {
                return this.asyncState;
            }
        }

        public T Result
        {
            get
            {
                lock (this)
                {
                    return result;
                }
            }
        }

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                lock (this)
                {
                    if (this.autoResetEvent == null)
                        this.autoResetEvent = new AutoResetEvent(false);

                    return this.autoResetEvent;
                }
            }
        }

        public bool CompletedSynchronously { get; private set; }

        public bool IsCompleted
        {
            get
            {
                return isCompleted;
            }
        }
    }
}
