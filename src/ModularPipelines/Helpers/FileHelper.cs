using System.Diagnostics;

namespace ModularPipelines.Helpers;

public static class FileHelper
{
    public static async Task WaitForFileAsync(string path)
    {
        var stopWatch = Stopwatch.StartNew();

        while (stopWatch.Elapsed < TimeSpan.FromSeconds(30))
        {
            await Task.Delay(500).ConfigureAwait(false);

            if (!File.Exists(path))
            {
                continue;
            }

            var fileInfo = new FileInfo(path);

            if (fileInfo.Length <= 0)
            {
                continue;
            }

            if (await IsFileLocked(fileInfo).ConfigureAwait(false))
            {
                continue;
            }

            break;
        }
    }

    public static async Task<bool> IsFileLocked(FileInfo file)
    {
        try
        {
            var stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            await using (stream.ConfigureAwait(false))
            {
                stream.Close();
            }
        }
        catch (IOException)
        {
            return true;
        }

        return false;
    }
}