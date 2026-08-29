using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using ModularPipelines.Logging;
using ModularPipelines.Serialization;

namespace ModularPipelines.FileSystem;

/// <summary>
/// Represents a file in the file system with extended functionality for pipeline operations.
/// </summary>
[JsonConverter(typeof(FilePathJsonConverter))]
public class FilePath : IEquatable<FilePath>
{
    [JsonIgnore]
    private readonly FileInfo _fileInfo;

    private readonly IFileSystemProvider _provider;

    private FileInfo FileInfo
    {
        get
        {
            _fileInfo.Refresh();
            return _fileInfo;
        }
    }

    public FilePath(string path) : this(new FileInfo(path), path, SystemFileSystemProvider.Instance)
    {
    }

    internal FilePath(FileInfo fileInfo) : this(fileInfo, fileInfo.FullName, SystemFileSystemProvider.Instance)
    {
    }

    internal FilePath(string path, IFileSystemProvider provider) : this(new FileInfo(path), path, provider)
    {
    }

    private FilePath(FileInfo fileInfo, string originalPath, IFileSystemProvider provider)
    {
        _fileInfo = fileInfo;
        OriginalPath = originalPath;
        _provider = provider;
    }

    /// <inheritdoc cref="System.IO.File.ReadAllTextAsync(string,System.Text.Encoding,System.Threading.CancellationToken)"/>>
    public Task<string> ReadAsync(CancellationToken cancellationToken = default)
    {
        LogFileOperation("Reading File: {Path}", this);

        return _provider.ReadAllTextAsync(Path, cancellationToken);
    }

    /// <inheritdoc cref="System.IO.File.ReadLinesAsync(string,System.Threading.CancellationToken)"/>
    public IAsyncEnumerable<string> ReadLinesAsync(CancellationToken cancellationToken = default)
    {
        LogFileOperation("Reading File: {Path}", this);

        return _provider.ReadLinesAsync(Path, cancellationToken);
    }

    public Task<byte[]> ReadBytesAsync(CancellationToken cancellationToken = default)
    {
        LogFileOperation("Reading File: {Path}", this);

        return _provider.ReadAllBytesAsync(Path, cancellationToken);
    }

    /// <summary>
    /// Opens a <see cref="Stream"/> on the current file with the specified access mode.
    /// </summary>
    /// <param name="fileAccess">The access mode for the file stream. Defaults to <see cref="FileAccess.ReadWrite"/>.</param>
    /// <returns>A <see cref="Stream"/> for the file.</returns>
    /// <remarks>
    /// <para>
    /// <strong>Important:</strong> The caller is responsible for disposing the returned <see cref="Stream"/>.
    /// Failure to dispose the stream will result in resource leaks and may prevent other operations on the file.
    /// </para>
    /// <para>
    /// Recommended usage with <c>await using</c> (async) or <c>using</c> (sync).
    /// </para>
    /// <example>
    /// <code>
    /// // Async usage (preferred)
    /// await using var stream = file.GetStream(FileAccess.Read);
    /// // Use the stream...
    ///
    /// // Sync usage
    /// using var stream = file.GetStream(FileAccess.Read);
    /// // Use the stream...
    /// </code>
    /// </example>
    /// </remarks>
    public Stream GetStream(FileAccess fileAccess = FileAccess.ReadWrite)
    {
        var fileMode = fileAccess switch
        {
            FileAccess.Read => FileMode.Open,
            FileAccess.Write => FileMode.Create,
            FileAccess.ReadWrite => FileMode.OpenOrCreate,
            _ => throw new ArgumentOutOfRangeException(nameof(fileAccess), fileAccess, null),
        };

        return _provider.Open(Path, fileMode, fileAccess);
    }

#pragma warning disable RS0026 // The v4 type rename intentionally preserves the established content-specific overloads.
    public Task WriteAsync(string contents, CancellationToken cancellationToken = default)
    {
        LogFileOperation("Writing to File: {Path}", this);

        return _provider.WriteAllTextAsync(Path, contents, cancellationToken);
    }

    public Task WriteAsync(byte[] contents, CancellationToken cancellationToken = default)
    {
        LogFileOperation("Writing to File: {Path}", this);

        return _provider.WriteAllBytesAsync(Path, contents, cancellationToken);
    }

    public Task WriteAsync(IEnumerable<string> contents, CancellationToken cancellationToken = default)
    {
        LogFileOperation("Writing to File: {Path}", this);

        return _provider.WriteAllLinesAsync(Path, contents, cancellationToken);
    }

    public async Task WriteAsync(ReadOnlyMemory<byte> contents, CancellationToken cancellationToken = default)
    {
        LogFileOperation("Writing to File: {Path}", this);

        var fileStream = _provider.Create(Path);
        await using (fileStream.ConfigureAwait(false))
        {
            await fileStream.WriteAsync(contents, cancellationToken).ConfigureAwait(false);
        }
    }

    public async Task WriteAsync(Stream contents, CancellationToken cancellationToken = default)
    {
        LogFileOperation("Writing to File: {Path}", this);

        var fileStream = _provider.Create(Path);
        await using (fileStream.ConfigureAwait(false))
        {
            if (contents.CanSeek)
            {
                contents.Position = 0;
            }

            await contents.CopyToAsync(fileStream, cancellationToken).ConfigureAwait(false);
        }
    }
#pragma warning restore RS0026

#pragma warning disable RS0026 // The v4 type rename intentionally preserves the established content-specific overloads.
    public Task AppendAsync(string contents, CancellationToken cancellationToken = default)
    {
        LogFileOperation("Appending to File: {Path}", this);

        return _provider.AppendAllTextAsync(Path, contents, cancellationToken);
    }

    public Task AppendAsync(IEnumerable<string> contents, CancellationToken cancellationToken = default)
    {
        LogFileOperation("Appending to File: {Path}", this);

        return _provider.AppendAllLinesAsync(Path, contents, cancellationToken);
    }
#pragma warning restore RS0026

    /// <inheritdoc cref="FileSystemInfo.Exists"/>>
    public bool Exists => _provider.FileExists(Path);

    public bool Hidden => (GetPhysicalFileInfo().Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;

    /// <inheritdoc cref="FileSystemInfo.Name"/>>
    public string Name => System.IO.Path.GetFileName(Path);

    /// <inheritdoc cref="System.IO.Path.GetFileNameWithoutExtension(System.ReadOnlySpan{char})"/>>
    public string NameWithoutExtension => System.IO.Path.GetFileNameWithoutExtension(this);

    /// <inheritdoc cref="FileInfo.Directory"/>>
    public FolderPath? Folder => System.IO.Path.GetDirectoryName(Path) is { } directory
        ? new FolderPath(directory, _provider)
        : null;

    /// <inheritdoc cref="FileSystemInfo.FullName"/>>
    public string Path => _fileInfo.FullName;

    /// <summary>
    /// Gets the original path string that was used to construct this FilePath instance.
    /// </summary>
    /// <remarks>
    /// Unlike <see cref="Path"/> which always returns the absolute path,
    /// this property preserves the original input (which may be relative).
    /// </remarks>
    public string OriginalPath { get; }

    public FilePath Create()
    {
        LogFileOperation("Creating File: {Path}", this);

        var fileStream = _provider.Create(Path);
        fileStream.Dispose();
        return this;
    }

    /// <summary>
    /// Asynchronously creates a new file at the current path.
    /// </summary>
    /// <returns>This file instance for method chaining.</returns>
    public async Task<FilePath> CreateAsync()
    {
        LogFileOperation("Creating File: {Path}", this);

        var fileStream = _provider.Create(Path);
        await fileStream.DisposeAsync().ConfigureAwait(false);
        return this;
    }

    /// <inheritdoc cref="FileSystemInfo.Attributes"/>>
    public FileAttributes Attributes
    {
        get { return GetPhysicalFileInfo().Attributes; }
        set { GetPhysicalFileInfo().Attributes = value; }
    }

    /// <inheritdoc cref="FileInfo.IsReadOnly"/>>
    public bool IsReadOnly => GetPhysicalFileInfo().IsReadOnly;

    /// <inheritdoc cref="FileSystemInfo.CreationTime"/>>
    public DateTimeOffset CreationTime => GetPhysicalFileInfo().CreationTime;

    public DateTimeOffset LastWriteTimeUtc => GetPhysicalFileInfo().LastWriteTimeUtc;

    /// <inheritdoc cref="FileSystemInfo.Extension"/>>
    public string Extension => System.IO.Path.GetExtension(Path);

    /// <inheritdoc cref="System.IO.FileInfo.Length"/>>
    public long Length => GetPhysicalFileInfo().Length;

    /// <inheritdoc cref="FileInfo.Delete"/>>
    public void Delete()
    {
        LogFileOperation("Deleting File: {Path}", this);

        _provider.DeleteFile(Path);
    }

    /// <summary>
    /// Asynchronously deletes the file.
    /// </summary>
    /// <remarks>
    /// Uses thread pool offloading as no native async delete API exists in .NET.
    /// For true async I/O, consider using stream-based operations where available.
    /// </remarks>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task that completes when the file has been deleted.</returns>
    public Task DeleteAsync(CancellationToken cancellationToken = default)
    {
        LogFileOperation("Deleting File: {Path}", this);

        return Task.Run(() => _provider.DeleteFile(Path), cancellationToken);
    }

    /// <inheritdoc cref="FileInfo.MoveTo(string)"/>>
    public FilePath MoveTo(string path)
    {
        LogFileOperationWithDestination("Moving File: {Source} > {Destination}", this, path);

        _provider.MoveFile(Path, path);
        return new FilePath(path, _provider);
    }

    /// <inheritdoc cref="FileInfo.MoveTo(string)"/>>
    public FilePath MoveTo(FolderPath folder)
    {
        LogFileOperationWithDestination("Moving File: {Source} > {Destination}", this, folder);

        folder.Create();
        return MoveTo(_provider.Combine(folder.Path, Name));
    }

    /// <summary>
    /// Asynchronously moves the file to a new path.
    /// </summary>
    /// <remarks>
    /// Uses thread pool offloading as no native async move API exists in .NET.
    /// </remarks>
    /// <param name="path">The destination path.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A new FilePath instance at the destination path.</returns>
#pragma warning disable RS0026 // The v4 type rename intentionally preserves path- and folder-specific overloads.
    public Task<FilePath> MoveToAsync(string path, CancellationToken cancellationToken = default)
    {
        LogFileOperationWithDestination("Moving File: {Source} > {Destination}", this, path);

        return Task.Run(() =>
        {
            _provider.MoveFile(Path, path);
            return new FilePath(path, _provider);
        }, cancellationToken);
    }

    /// <summary>
    /// Asynchronously moves the file to a folder.
    /// </summary>
    /// <remarks>
    /// Uses thread pool offloading as no native async move API exists in .NET.
    /// </remarks>
    /// <param name="folder">The destination folder.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A new FilePath instance at the destination path.</returns>
    public async Task<FilePath> MoveToAsync(FolderPath folder, CancellationToken cancellationToken = default)
    {
        LogFileOperationWithDestination("Moving File: {Source} > {Destination}", this, folder);

        await folder.CreateAsync().ConfigureAwait(false);
        return await MoveToAsync(_provider.Combine(folder.Path, Name), cancellationToken).ConfigureAwait(false);
    }
#pragma warning restore RS0026

    /// <inheritdoc cref="FileInfo.CopyTo(string)"/>>
    public FilePath CopyTo(string path)
    {
        LogFileOperationWithDestination("Copying File: {Source} > {Destination}", this, path);

        _provider.CopyFile(Path, path, true);
        return new FilePath(path, _provider);
    }

    public FilePath CopyTo(FolderPath folder)
    {
        LogFileOperationWithDestination("Copying File: {Source} > {Destination}", this, folder);

        folder.Create();
        return CopyTo(_provider.Combine(folder.Path, Name));
    }

    /// <summary>
    /// Asynchronously copies the file to a new path using stream-based copying.
    /// </summary>
    /// <param name="path">The destination path.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A new FilePath instance representing the copied file.</returns>
#pragma warning disable RS0026 // The v4 type rename intentionally preserves path- and folder-specific overloads.
    public async Task<FilePath> CopyToAsync(string path, CancellationToken cancellationToken = default)
    {
        LogFileOperationWithDestination("Copying File: {Source} > {Destination}", this, path);

        var sourceStream = _provider.OpenRead(Path);
        await using (sourceStream.ConfigureAwait(false))
        {
            var destStream = _provider.Create(path);
            await using (destStream.ConfigureAwait(false))
            {
                await sourceStream.CopyToAsync(destStream, cancellationToken).ConfigureAwait(false);
            }
        }

        return new FilePath(path, _provider);
    }

    /// <summary>
    /// Asynchronously copies the file to a folder using stream-based copying.
    /// </summary>
    /// <param name="folder">The destination folder.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A new FilePath instance representing the copied file.</returns>
    public async Task<FilePath> CopyToAsync(FolderPath folder, CancellationToken cancellationToken = default)
    {
        LogFileOperationWithDestination("Copying File: {Source} > {Destination}", this, folder);

        await folder.CreateAsync().ConfigureAwait(false);
        return await CopyToAsync(_provider.Combine(folder.Path, Name), cancellationToken).ConfigureAwait(false);
    }
#pragma warning restore RS0026

    public static FilePath GetNewTemporaryFilePath()
    {
        var provider = SystemFileSystemProvider.Instance;
        var path = provider.Combine(provider.GetTempPath(), provider.GetRandomFileName());

        LogFileOperation("Temporary File Path: {Path}", path);

        return path!;
    }

    public static implicit operator FilePath?(string? path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return null;
        }

        return new FileInfo(path);
    }

    [return: NotNullIfNotNull("fileInfo")]
    public static implicit operator FilePath?(FileInfo? fileInfo)
    {
        if (fileInfo == null)
        {
            return null;
        }

        return new FilePath(fileInfo);
    }

    [return: NotNullIfNotNull(parameterName: "file")]
    public static implicit operator string?(FilePath? file)
    {
        return file?.Path;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Path;
    }

    /// <inheritdoc/>
    public bool Equals(FilePath? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (OperatingSystem.IsWindows())
        {
            return string.Equals(Path, other.Path, StringComparison.OrdinalIgnoreCase);
        }

        return Path == other.Path;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj is FilePath other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        if (OperatingSystem.IsWindows())
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(Path);
        }

        return Path.GetHashCode();
    }

    public static bool operator ==(FilePath? left, FilePath? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(FilePath? left, FilePath? right)
    {
        return !Equals(left, right);
    }

    private FileInfo GetPhysicalFileInfo()
    {
        if (!ReferenceEquals(_provider, SystemFileSystemProvider.Instance))
        {
            throw new NotSupportedException(
                "File metadata is unavailable through the configured IFileSystemProvider.");
        }

        return FileInfo;
    }

    /// <summary>
    /// Logs a file operation.
    /// </summary>
    private static void LogFileOperation(string messageTemplate, object? arg1)
    {
        ModuleLogger.Current.LogInformation(messageTemplate, arg1);
    }

    /// <summary>
    /// Logs a file operation for operations with source and destination.
    /// </summary>
    private static void LogFileOperationWithDestination(string messageTemplate, object? source, object? destination)
    {
        ModuleLogger.Current.LogInformation(messageTemplate, source, destination);
    }
}
