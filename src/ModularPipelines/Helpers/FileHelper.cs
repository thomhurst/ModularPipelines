using System.Diagnostics;

namespace ModularPipelines.Helpers;

public static class FileHelper
{
    public static async Task WaitForFileAsync(string path)
    {
        var stopWatch = Stopwatch.StartNew();
        
        while (stopWatch.Elapsed < TimeSpan.FromSeconds(30))
        {
            await Task.Delay(500);
            
            if (!File.Exists(path))
            {
                continue;
            }

            var fileInfo = new FileInfo(path);
            
            if (fileInfo.Length <= 0)
            {
                continue;
            }

            if (IsFileLocked(fileInfo))
            {
                continue;
            }
            
            break;
        }
    }
    
    public static bool IsFileLocked(FileInfo file)
    {
        try
        {
            using var stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            stream.Close();
        }
        catch (IOException)
        {
            return true;
        }

        return false;
    }

    public static string GetTempFileName()
    {
        return Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.tmp");
    }
}