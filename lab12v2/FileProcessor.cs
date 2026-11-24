using System;
using System.IO;

namespace Lab12v2;

public class FileProcessor
{
    private int _attempts = 0;

    public void WriteToFile(string path, string content)
    {
        _attempts++;
        Console.WriteLine($"[FileProcessor] Виклик WriteToFile (Спроба {_attempts})...");

        if (_attempts <= 3)
        {
            throw new IOException($"Імітація помилки вводу/виводу при записі у файл '{path}'.");
        }

        Console.WriteLine($"[FileProcessor] Успішно записано дані у файл: {path}");
        _attempts = 0;
    }
}
