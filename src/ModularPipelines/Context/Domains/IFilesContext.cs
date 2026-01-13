using ModularPipelines.Context.Domains.Files;
using ModularPipelines.FileSystem;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Context.Domains;

/// <summary>
/// Provides file system operations with rich File and Folder return types.
/// </summary>
public interface IFilesContext
{
    /// <summary>
    /// Get a File object for the specified path.
    /// </summary>
    File GetFile(string path);

    /// <summary>
    /// Get a Folder object for the specified path.
    /// </summary>
    Folder GetFolder(string path);

    /// <summary>
    /// Search for files matching a glob pattern. Returns rich File objects.
    /// </summary>
    IEnumerable<File> Glob(string pattern);

    /// <summary>
    /// Search for folders matching a glob pattern.
    /// </summary>
    IEnumerable<Folder> GlobFolders(string pattern);

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

    /// <summary>
    /// File checksum operations.
    /// </summary>
    IChecksumContext Checksum { get; }
}
