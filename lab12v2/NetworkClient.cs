using System;
using System.Net.Http;

namespace Lab12v2;

public class NetworkClient
{
    private int _attempts = 0;

    public bool UploadFile(string url, string filePath)
    {
        _attempts++;
        Console.WriteLine($"[NetworkClient] Виклик UploadFile (Спроба {_attempts})...");

        if (_attempts <= 2)
        {
            throw new HttpRequestException($"Імітація мережевої помилки при відправці на {url}.");
        }

        Console.WriteLine($"[NetworkClient] Успішно завантажено файл '{filePath}' на {url}");
        _attempts = 0; 
        return true;
    }
}
