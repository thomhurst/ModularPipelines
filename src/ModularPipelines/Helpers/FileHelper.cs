using System.Diagnostics;

namespace ModularPipelines.Helpers;

/// <summary>
/// Helper methods for file system operations.
/// </summary>
public static class FileHelper
{
    /// <summary>
    /// Default timeout for waiting for a file.
    /// </summary>
    public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Default polling interval when checking for file availability.
    /// </summary>
    public static readonly TimeSpan DefaultPollingInterval = TimeSpan.FromMilliseconds(500);

    /// <summary>
    /// Waits for a file to exist, have content, and be unlocked.
    /// </summary>
    /// <param name="path">The path to the file to wait for.</param>
    /// <param name="timeout">
    /// The maximum time to wait. If null, defaults to <see cref="DefaultTimeout"/>.
    /// </param>
    /// <param name="pollingInterval">
    /// The interval between checks. If null, defaults to <see cref="DefaultPollingInterval"/>.
    /// </param>
    /// <param name="cancellationToken">A token to cancel the wait operation.</param>
    /// <returns>
    /// True if the file became available within the timeout; false if timed out.
    /// </returns>
    public static async Task<bool> WaitForFileAsync(
        string path,
        TimeSpan? timeout = null,
        TimeSpan? pollingInterval = null,
        CancellationToken cancellationToken = default)
    {
        var actualTimeout = timeout ?? DefaultTimeout;
        var actualPollingInterval = pollingInterval ?? DefaultPollingInterval;
        var stopWatch = Stopwatch.StartNew();

        while (stopWatch.Elapsed < actualTimeout)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await Task.Delay(actualPollingInterval, cancellationToken).ConfigureAwait(false);

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

            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if a file is currently locked by another process.
    /// </summary>
    /// <param name="file">The file to check.</param>
    /// <returns>True if the file is locked; otherwise, false.</returns>
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