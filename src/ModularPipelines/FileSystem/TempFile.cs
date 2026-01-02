namespace ModularPipelines.FileSystem;

/// <summary>
/// Represents a temporary file that is automatically deleted when disposed.
/// </summary>
/// <remarks>
/// <para>
/// This class provides a safe way to work with temporary files by ensuring
/// automatic cleanup when the instance is disposed, either explicitly or
/// when used with <c>await using</c>.
/// </para>
/// <para>
/// Example usage:
/// </para>
/// <example>
/// <code>
/// await using var tempFile = new TempFile();
/// await tempFile.File.WriteAsync("content");
/// // File is automatically deleted when disposed
/// </code>
/// </example>
/// </remarks>
public sealed class TempFile : IAsyncDisposable, IDisposable
{
    private int _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="TempFile"/> class with a new temporary file path.
    /// </summary>
    public TempFile()
    {
        File = File.GetNewTemporaryFilePath();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TempFile"/> class with an existing file.
    /// </summary>
    /// <param name="file">The file to wrap as a temporary file.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is null.</exception>
    public TempFile(File file)
    {
        ArgumentNullException.ThrowIfNull(file);
        File = file;
    }

    /// <summary>
    /// Gets the underlying temporary file.
    /// </summary>
    public File File { get; }

    /// <summary>
    /// Asynchronously disposes the temporary file, deleting it if it exists.
    /// </summary>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    public async ValueTask DisposeAsync()
    {
        if (Interlocked.Exchange(ref _disposed, 1) == 1)
        {
            return;
        }

        if (File.Exists)
        {
            await File.DeleteAsync().ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Disposes the temporary file, deleting it if it exists.
    /// </summary>
    public void Dispose()
    {
        if (Interlocked.Exchange(ref _disposed, 1) == 1)
        {
            return;
        }

        if (File.Exists)
        {
            File.Delete();
        }
    }
}
