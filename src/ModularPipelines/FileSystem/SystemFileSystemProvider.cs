using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.FileSystem;

/// <summary>
/// Default implementation that delegates to System.IO.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class SystemFileSystemProvider : IFileSystemProvider
{
    /// <summary>
    /// Gets singleton instance for use when no DI is available.
    /// </summary>
    public static SystemFileSystemProvider Instance { get; } = new();

    private SystemFileSystemProvider()
    {
    }

    /// <inheritdoc />
    public Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default)
        => System.IO.File.ReadAllTextAsync(path, cancellationToken);

    /// <inheritdoc />
    public IAsyncEnumerable<string> ReadLinesAsync(string path, CancellationToken cancellationToken = default)
        => System.IO.File.ReadLinesAsync(path, cancellationToken);

    /// <inheritdoc />
    public Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken = default)
        => System.IO.File.ReadAllBytesAsync(path, cancellationToken);

    /// <inheritdoc />
    public Task WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken = default)
        => System.IO.File.WriteAllTextAsync(path, contents, cancellationToken);

    /// <inheritdoc />
    public Task WriteAllBytesAsync(string path, byte[] contents, CancellationToken cancellationToken = default)
        => System.IO.File.WriteAllBytesAsync(path, contents, cancellationToken);

    /// <inheritdoc />
    public Task WriteAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default)
        => System.IO.File.WriteAllLinesAsync(path, contents, cancellationToken);

    /// <inheritdoc />
    public Task AppendAllTextAsync(string path, string contents, CancellationToken cancellationToken = default)
        => System.IO.File.AppendAllTextAsync(path, contents, cancellationToken);

    /// <inheritdoc />
    public Task AppendAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default)
        => System.IO.File.AppendAllLinesAsync(path, contents, cancellationToken);

    /// <inheritdoc />
    public Stream OpenRead(string path)
        => System.IO.File.OpenRead(path);

    /// <inheritdoc />
    public Stream Create(string path)
        => System.IO.File.Create(path);

    /// <inheritdoc />
    public Stream Open(string path, FileMode mode, FileAccess access)
        => System.IO.File.Open(path, mode, access);

    /// <inheritdoc />
    public void DeleteFile(string path)
        => System.IO.File.Delete(path);

    /// <inheritdoc />
    public void CopyFile(string sourcePath, string destinationPath, bool overwrite)
        => System.IO.File.Copy(sourcePath, destinationPath, overwrite);

    /// <inheritdoc />
    public void MoveFile(string sourcePath, string destinationPath)
        => System.IO.File.Move(sourcePath, destinationPath);

    /// <inheritdoc />
    public void MoveFile(string sourcePath, string destinationPath, bool overwrite)
        => System.IO.File.Move(sourcePath, destinationPath, overwrite);

    /// <inheritdoc />
    public bool FileExists(string path)
        => System.IO.File.Exists(path);

    /// <inheritdoc />
    public void CreateDirectory(string path)
        => Directory.CreateDirectory(path);

    /// <inheritdoc />
    public void DeleteDirectory(string path, bool recursive)
        => Directory.Delete(path, recursive);

    /// <inheritdoc />
    public void MoveDirectory(string sourcePath, string destinationPath)
        => Directory.Move(sourcePath, destinationPath);

    /// <inheritdoc />
    public bool DirectoryExists(string path)
        => Directory.Exists(path);

    /// <inheritdoc />
    public IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
        => Directory.EnumerateFiles(path, searchPattern, searchOption);

    /// <inheritdoc />
    public IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
        => Directory.EnumerateDirectories(path, searchPattern, searchOption);

    /// <inheritdoc />
    public FileAttributes GetAttributes(string path)
        => GetInfo(path).Attributes;

    /// <inheritdoc />
    public void SetAttributes(string path, FileAttributes attributes)
        => GetInfo(path).Attributes = attributes;

    /// <inheritdoc />
    public DateTime GetCreationTimeUtc(string path)
        => GetInfo(path).CreationTimeUtc;

    /// <inheritdoc />
    public void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
        => GetInfo(path).CreationTimeUtc = creationTimeUtc;

    /// <inheritdoc />
    public DateTime GetLastWriteTimeUtc(string path)
        => GetInfo(path).LastWriteTimeUtc;

    /// <inheritdoc />
    public void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        => GetInfo(path).LastWriteTimeUtc = lastWriteTimeUtc;

    /// <inheritdoc />
    public DateTime GetLastAccessTimeUtc(string path)
        => GetInfo(path).LastAccessTimeUtc;

    /// <inheritdoc />
    public void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        => GetInfo(path).LastAccessTimeUtc = lastAccessTimeUtc;

    /// <inheritdoc />
    public long GetFileLength(string path)
        => new FileInfo(path).Length;

    /// <inheritdoc />
    public string GetTempPath()
        => Path.GetTempPath();

    /// <inheritdoc />
    public string GetRandomFileName()
        => Path.GetRandomFileName();

    /// <inheritdoc />
    public string Combine(params string[] paths)
        => Path.Combine(paths);

    /// <inheritdoc />
    public string GetRelativePath(string relativeTo, string path)
        => Path.GetRelativePath(relativeTo, path);

    private static FileSystemInfo GetInfo(string path)
        => Directory.Exists(path) ? new DirectoryInfo(path) : new FileInfo(path);
}
