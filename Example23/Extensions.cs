using System;
using System.Collections.Generic;

namespace Example23
{
    public static class Extensions
    {
        public static IEnumerable<Exception> FlattenHierarchy(this Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("Exception is null!");
            }

            do
            {
                yield return exception;
                exception = exception.InnerException;
            }
            while (exception != null);
        }
    }
}
