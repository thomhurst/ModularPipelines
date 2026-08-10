namespace ModularPipelines.Engine;

internal static class AtomicFileWriter
{
    private const string TemporaryFilePrefix = ".modularpipelines-";
    private const string TemporaryFileSuffix = ".tmp";
    internal const string TemporaryFilePattern = TemporaryFilePrefix + "*" + TemporaryFileSuffix;

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

    internal static Task WriteAllTextAsync(
        string path,
        string contents,
        Func<string, string, CancellationToken, Task> writeAsync,
        CancellationToken cancellationToken = default) =>
        WriteAllTextAsync(
            path,
            contents,
            writeAsync,
            replacementContentsFactory: null,
            tryPublish: null,
            cancellationToken);

    internal static async Task WriteAllTextAsync(
        string path,
        string contents,
        Func<string, string, CancellationToken, Task> writeAsync,
        Func<string?>? replacementContentsFactory,
        Func<Action, bool>? tryPublish = null,
        CancellationToken cancellationToken = default)
    {
        var directory = Path.GetDirectoryName(path)
            ?? throw new ArgumentException("The path must include a directory.", nameof(path));
        var temporaryPath = Path.Combine(
            directory,
            $"{TemporaryFilePrefix}{Guid.NewGuid():N}{TemporaryFileSuffix}");

        try
        {
            await writeAsync(temporaryPath, contents, cancellationToken).ConfigureAwait(false);
            var replacementContents = replacementContentsFactory?.Invoke();
            if (replacementContents is not null)
            {
                await writeAsync(temporaryPath, replacementContents, cancellationToken)
                    .ConfigureAwait(false);
            }

            cancellationToken.ThrowIfCancellationRequested();
            if (replacementContents is not null || tryPublish is null)
            {
                File.Move(temporaryPath, path, overwrite: true);
            }
            else if (!tryPublish(() => File.Move(temporaryPath, path, overwrite: true)))
            {
                replacementContents = replacementContentsFactory?.Invoke()
                    ?? throw new InvalidOperationException(
                        "Atomic publication validation failed without safe replacement contents.");
                await writeAsync(temporaryPath, replacementContents, cancellationToken)
                    .ConfigureAwait(false);
                cancellationToken.ThrowIfCancellationRequested();
                File.Move(temporaryPath, path, overwrite: true);
            }
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
