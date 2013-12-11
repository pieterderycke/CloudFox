using System;
using System.Threading;

namespace CloudFox.Util
{
    public static class AsyncHelper
    {
        public static IAsyncResult BeginInvoke<T>(Func<T> function, AsyncCallback callback, object state)
        {
            AsyncResult<T> asyncResult = new AsyncResult<T>(callback, state);

            ThreadPool.QueueUserWorkItem(o =>
            {
                try
                {
                    T result = function();
                    asyncResult.SetAsCompleted(result, false);
                }
                catch (Exception ex)
                {
                    asyncResult.SetAsCompleted(ex, false);
                }
            });

            return asyncResult;
        }

        public static T EndInvoke<T>(IAsyncResult asyncResult)
        {
            if (asyncResult == null)
                throw new ArgumentNullException("asyncResult");

            AsyncResult<T> internalAsyncResult = asyncResult as AsyncResult<T>;

            if (internalAsyncResult == null)
                throw new ArgumentException("Invalid IAsyncResult instance.", "asyncResult");

            internalAsyncResult.EndInvoke();

            return internalAsyncResult.Result;
        }
    }
}
