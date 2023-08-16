namespace NotificationSystem.Common.Utils
{
    public static class Retry
    {
        public static async Task Execute(
            Func<Task> action,
            string actionName,
            int maximumRetryCount,
            int delaySeedSeconds)
        {
            var successful = false;
            var currentRetries = 0;
            var delaySeconds = 1;
            while (!successful)
            {
                try
                {
                    await action();
                    successful = true;
                }
                catch (Exception exception)
                {
                    currentRetries++;
                    await RetryOperation(currentRetries, maximumRetryCount, delaySeconds, delaySeedSeconds, exception, actionName);
                }
            }
        }

        private static async Task RetryOperation(
            int currentRetries,
            int maximumRetryCount,
            int delaySeconds,
            int delaySeedSeconds,
            Exception exception,
            string actionName)
        {
            if (currentRetries < maximumRetryCount)
            {
                delaySeconds *= delaySeedSeconds;

                var delayMilliseconds = delaySeconds * 1000;
                await Task.Delay(delayMilliseconds);
            }
            else
            {
                throw exception;
            }
        }
    }
}
