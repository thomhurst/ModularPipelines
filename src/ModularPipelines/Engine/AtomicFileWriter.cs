namespace ModularPipelines.Engine;

internal static class AtomicFileWriter
{
    public static Task WriteAllTextAsync(
        string path,
        string contents,
        CancellationToken cancellationToken = default) =>
        WriteAllTextAsync(
            path,
            contents,
            static (temporaryPath, text, token) =>
                File.WriteAllTextAsync(temporaryPath, text, token),
            cancellationToken);

    internal static async Task WriteAllTextAsync(
        string path,
        string contents,
        Func<string, string, CancellationToken, Task> writeAsync,
        CancellationToken cancellationToken = default)
    {
        var directory = Path.GetDirectoryName(path)
            ?? throw new ArgumentException("The path must include a directory.", nameof(path));
        var temporaryPath = Path.Combine(
            directory,
            $".modularpipelines-{Guid.NewGuid():N}.tmp");

        try
        {
            await writeAsync(temporaryPath, contents, cancellationToken).ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
            File.Move(temporaryPath, path, overwrite: true);
        }
        finally
        {
            TryDelete(temporaryPath);
        }
    }

    private static void TryDelete(string path)
    {
        try
        {
            File.Delete(path);
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            // Preserve the write or move failure. An unpublished temp file is safe to remove later.
        }
    }
}
