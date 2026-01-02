namespace ModularPipelines.FileSystem;

/// <summary>
/// Represents a temporary folder that is automatically deleted when disposed.
/// </summary>
/// <remarks>
/// <para>
/// This class provides a safe way to work with temporary folders by ensuring
/// automatic cleanup when the instance is disposed, either explicitly or
/// when used with <c>await using</c>.
/// </para>
/// <para>
/// Example usage:
/// </para>
/// <example>
/// <code>
/// await using var tempFolder = new TempFolder();
/// var file = tempFolder.Folder.CreateFile("test.txt");
/// // Folder and all contents are automatically deleted when disposed
/// </code>
/// </example>
/// </remarks>
public sealed class TempFolder : IAsyncDisposable, IDisposable
{
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="TempFolder"/> class with a new temporary folder.
    /// </summary>
    public TempFolder()
    {
        Folder = Folder.CreateTemporaryFolder();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TempFolder"/> class with an existing folder.
    /// </summary>
    /// <param name="folder">The folder to wrap as a temporary folder.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="folder"/> is null.</exception>
    public TempFolder(Folder folder)
    {
        ArgumentNullException.ThrowIfNull(folder);
        Folder = folder;
    }

    /// <summary>
    /// Gets the underlying temporary folder.
    /// </summary>
    public Folder Folder { get; }

    /// <summary>
    /// Asynchronously disposes the temporary folder, recursively deleting it and all its contents if it exists.
    /// </summary>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    public async ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        if (Folder.Exists)
        {
            await Folder.DeleteAsync().ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Disposes the temporary folder, recursively deleting it and all its contents if it exists.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        if (Folder.Exists)
        {
            Folder.Delete();
        }
    }
}
