using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.FileSystem;

/// <summary>
/// Default implementation that delegates to System.IO.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class SystemFileSystemProvider : IFileSystemProvider
{
    /// <summary>
    /// Singleton instance for use when no DI is available.
    /// </summary>
    public static SystemFileSystemProvider Instance { get; } = new();

    private SystemFileSystemProvider() { }

    // File read operations
    public Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default)
        => System.IO.File.ReadAllTextAsync(path, cancellationToken);

    public IAsyncEnumerable<string> ReadLinesAsync(string path, CancellationToken cancellationToken = default)
        => System.IO.File.ReadLinesAsync(path, cancellationToken);

    public Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken = default)
        => System.IO.File.ReadAllBytesAsync(path, cancellationToken);

    // File write operations
    public Task WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken = default)
        => System.IO.File.WriteAllTextAsync(path, contents, cancellationToken);

    public Task WriteAllBytesAsync(string path, byte[] contents, CancellationToken cancellationToken = default)
        => System.IO.File.WriteAllBytesAsync(path, contents, cancellationToken);

    public Task WriteAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default)
        => System.IO.File.WriteAllLinesAsync(path, contents, cancellationToken);

    public Task AppendAllTextAsync(string path, string contents, CancellationToken cancellationToken = default)
        => System.IO.File.AppendAllTextAsync(path, contents, cancellationToken);

    public Task AppendAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default)
        => System.IO.File.AppendAllLinesAsync(path, contents, cancellationToken);

    // File stream operations
    public Stream OpenRead(string path)
        => System.IO.File.OpenRead(path);

    public Stream Create(string path)
        => System.IO.File.Create(path);

    public Stream Open(string path, FileMode mode, FileAccess access)
        => System.IO.File.Open(path, mode, access);

    // File management operations
    public void DeleteFile(string path)
        => System.IO.File.Delete(path);

    public void CopyFile(string sourcePath, string destinationPath, bool overwrite)
        => System.IO.File.Copy(sourcePath, destinationPath, overwrite);

    public void MoveFile(string sourcePath, string destinationPath)
        => System.IO.File.Move(sourcePath, destinationPath);

    public bool FileExists(string path)
        => System.IO.File.Exists(path);

    // Directory operations
    public void CreateDirectory(string path)
        => Directory.CreateDirectory(path);

    public void DeleteDirectory(string path, bool recursive)
        => Directory.Delete(path, recursive);

    public void MoveDirectory(string sourcePath, string destinationPath)
        => Directory.Move(sourcePath, destinationPath);

    public bool DirectoryExists(string path)
        => Directory.Exists(path);

    public IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
        => Directory.EnumerateFiles(path, searchPattern, searchOption);

    public IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
        => Directory.EnumerateDirectories(path, searchPattern, searchOption);

    // Path operations
    public string GetTempPath()
        => Path.GetTempPath();

    public string GetRandomFileName()
        => Path.GetRandomFileName();

    public string Combine(params string[] paths)
        => Path.Combine(paths);

    public string GetRelativePath(string relativeTo, string path)
        => Path.GetRelativePath(relativeTo, path);
}
