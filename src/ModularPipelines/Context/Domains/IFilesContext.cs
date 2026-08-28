using ModularPipelines.Context.Domains.Files;
using ModularPipelines.FileSystem;

namespace ModularPipelines.Context.Domains;

/// <summary>
/// Provides file system operations with rich FilePath and FolderPath return types.
/// </summary>
public interface IFilesContext
{
    /// <summary>
    /// Get a FilePath object for the specified path.
    /// </summary>
    FilePath GetFile(string path);

    /// <summary>
    /// Get a FolderPath object for the specified path.
    /// </summary>
    FolderPath GetFolder(string path);

    /// <summary>
    /// Get a FolderPath object for the specified special folder.
    /// </summary>
    FolderPath GetFolder(System.Environment.SpecialFolder specialFolder);

    /// <summary>
    /// Search for files matching a glob pattern. Returns rich FilePath objects.
    /// </summary>
    IEnumerable<FilePath> Glob(string pattern);

    /// <summary>
    /// Search for folders matching a glob pattern.
    /// </summary>
    IEnumerable<FolderPath> GlobFolders(string pattern);

    /// <summary>
    /// Read file contents as string.
    /// </summary>
    Task<string> ReadAsync(string path, CancellationToken cancellationToken = default);

    /// <summary>
    /// Write string content to a file.
    /// </summary>
    Task WriteAsync(string path, string content, CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if a file exists.
    /// </summary>
    Task<bool> ExistsAsync(string path, CancellationToken cancellationToken = default);

    /// <summary>
    /// Compression operations.
    /// </summary>
    IZipContext Zip { get; }
}
