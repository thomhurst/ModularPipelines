namespace ModularPipelines.FileSystem;

/// <summary>
/// Provides low-level file system operations.
/// Inject a mock implementation for testing file system interactions.
/// </summary>
public interface IFileSystemProvider
{
    // File read operations
    Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default);
    IAsyncEnumerable<string> ReadLinesAsync(string path, CancellationToken cancellationToken = default);
    Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken = default);

    // File write operations
    Task WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken = default);
    Task WriteAllBytesAsync(string path, byte[] contents, CancellationToken cancellationToken = default);
    Task WriteAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default);
    Task AppendAllTextAsync(string path, string contents, CancellationToken cancellationToken = default);
    Task AppendAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default);

    // File stream operations
    Stream OpenRead(string path);
    Stream Create(string path);
    Stream Open(string path, FileMode mode, FileAccess access);

    // File management operations
    void DeleteFile(string path);
    void CopyFile(string sourcePath, string destinationPath, bool overwrite);
    void MoveFile(string sourcePath, string destinationPath);
    bool FileExists(string path);

    // Directory operations
    void CreateDirectory(string path);
    void DeleteDirectory(string path, bool recursive);
    void MoveDirectory(string sourcePath, string destinationPath);
    bool DirectoryExists(string path);
    IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption);
    IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption);

    // Path operations
    string GetTempPath();
    string GetRandomFileName();
    string Combine(params string[] paths);
    string GetRelativePath(string relativeTo, string path);
}
