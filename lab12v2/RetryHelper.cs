using System;
using System.Threading;

namespace Lab12v2;

public static class RetryHelper
{
    public static T ExecuteWithRetry<T>(
        Func<T> operation,
        int retryCount = 3,
        TimeSpan initialDelay = default,
        Func<Exception, bool>? shouldRetry = null)
    {
        int attempt = 0;
        TimeSpan currentDelay = initialDelay == default ? TimeSpan.FromSeconds(1) : initialDelay;

        while (true)
        {
            try
            {
                return operation();
            }
            catch (Exception ex)
            {
                attempt++;

                if (attempt > retryCount || (shouldRetry != null && !shouldRetry(ex)))
                {
                    Console.WriteLine($"[RetryHelper] Остаточна помилка після {attempt} спроб(и): {ex.Message}");
                    throw;
                }

                Console.WriteLine($"[RetryHelper] Спроба {attempt} провалилася ({ex.GetType().Name}). Повтор через {currentDelay.TotalSeconds} сек...");

                Thread.Sleep(currentDelay);

                currentDelay = TimeSpan.FromMilliseconds(currentDelay.TotalMilliseconds * 2);
            }
        }
    }
}
