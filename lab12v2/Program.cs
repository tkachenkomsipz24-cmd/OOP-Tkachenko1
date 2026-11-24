using System;
using System.IO;
using System.Net.Http;
using System.Text;

namespace Lab12v2;

public static class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        var fileProcessor = new FileProcessor();
        var networkClient = new NetworkClient();

        Func<Exception, bool> shouldRetry = ex =>
            ex is IOException || ex is HttpRequestException;

        Console.WriteLine("=== 1. Тестування FileProcessor (IOException 3 рази, на 4-й успіх) ===");
        try
        {
            RetryHelper.ExecuteWithRetry<bool>(() =>
            {
                fileProcessor.WriteToFile("data.txt", "Важливі дані");
                return true; // Метод void, тому повертаємо true для Func<T>
            }, retryCount: 4, initialDelay: TimeSpan.FromSeconds(0.5), shouldRetry: shouldRetry);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Операція скасована: {ex.Message}");
        }

        Console.WriteLine("\n=== 2. Тестування NetworkClient (HttpRequestException 2 рази, на 3-й успіх) ===");
        try
        {
            RetryHelper.ExecuteWithRetry<bool>(() =>
            {
                return networkClient.UploadFile("https://api.kpi.ua/upload", "data.txt");
            }, retryCount: 3, initialDelay: TimeSpan.FromSeconds(0.5), shouldRetry: shouldRetry);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Операція скасована: {ex.Message}");
        }

        Console.WriteLine("\n=== 3. Невідомий виняток (shouldRetry повертає false -> повтору немає) ===");
        int calls = 0;
        try
        {
            RetryHelper.ExecuteWithRetry<bool>(() =>
            {
                calls++;
                Console.WriteLine($"[Program] Виклик операції (Спроба {calls})...");
                throw new InvalidOperationException("Імітація логічної помилки, яку повторювати марно.");
            }, retryCount: 3, initialDelay: TimeSpan.FromSeconds(0.5), shouldRetry: shouldRetry);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Операція скасована ({ex.GetType().Name}): {ex.Message}");
            Console.WriteLine($"Усього викликів операції: {calls} (повторів не було).");
        }
    }
}
