using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using ModularPipelines.Logging;
using ModularPipelines.Tracing;

namespace ModularPipelines.FileSystem;

/// <summary>
/// Represents a file in the file system with extended functionality for pipeline operations.
/// </summary>
public class File : IEquatable<File>
{
    [JsonIgnore]
    private readonly FileInfo _fileInfo;

    private FileInfo FileInfo
    {
        get
        {
            _fileInfo.Refresh();
            return _fileInfo;
        }
    }

    public File(string path) : this(new FileInfo(path), path)
    {
    }

    internal File(FileInfo fileInfo) : this(fileInfo, fileInfo.FullName)
    {
    }

    private File(FileInfo fileInfo, string originalPath)
    {
        _fileInfo = fileInfo;
        OriginalPath = originalPath;
    }

    /// <inheritdoc cref="System.IO.File.ReadAllTextAsync(string,System.Text.Encoding,System.Threading.CancellationToken)"/>>
    public Task<string> ReadAsync(CancellationToken cancellationToken = default)
    {
        LogFileOperation("Reading File: {Path} [Module: {ModuleName}, Activity: {ActivityId}]", this);

        return System.IO.File.ReadAllTextAsync(Path, cancellationToken);
    }

    /// <inheritdoc cref="System.IO.File.ReadLinesAsync(string,System.Threading.CancellationToken)"/>
    public IAsyncEnumerable<string> ReadLinesAsync(CancellationToken cancellationToken = default)
    {
        LogFileOperation("Reading File: {Path} [Module: {ModuleName}, Activity: {ActivityId}]", this);

        return System.IO.File.ReadLinesAsync(Path, cancellationToken);
    }

    public Task<byte[]> ReadBytesAsync(CancellationToken cancellationToken = default)
    {
        LogFileOperation("Reading File: {Path} [Module: {ModuleName}, Activity: {ActivityId}]", this);

        return System.IO.File.ReadAllBytesAsync(Path, cancellationToken);
    }

    /// <summary>
    /// Opens a <see cref="FileStream"/> on the current file with the specified access mode.
    /// </summary>
    /// <param name="fileAccess">The access mode for the file stream. Defaults to <see cref="FileAccess.ReadWrite"/>.</param>
    /// <returns>A <see cref="FileStream"/> for the file.</returns>
    /// <remarks>
    /// <para>
    /// <strong>Important:</strong> The caller is responsible for disposing the returned <see cref="FileStream"/>.
    /// Failure to dispose the stream will result in resource leaks and may prevent other operations on the file.
    /// </para>
    /// <para>
    /// Recommended usage with <c>await using</c> (async) or <c>using</c> (sync):
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
    public FileStream GetStream(FileAccess fileAccess = FileAccess.ReadWrite)
    {
        return System.IO.File.Open(Path, FileMode.OpenOrCreate, fileAccess);
    }

    public Task WriteAsync(string contents, CancellationToken cancellationToken = default)
    {
        LogFileOperation("Writing to File: {Path} [Module: {ModuleName}, Activity: {ActivityId}]", this);

        return System.IO.File.WriteAllTextAsync(Path, contents, cancellationToken);
    }

    public Task WriteAsync(byte[] contents, CancellationToken cancellationToken = default)
    {
        LogFileOperation("Writing to File: {Path} [Module: {ModuleName}, Activity: {ActivityId}]", this);

        return System.IO.File.WriteAllBytesAsync(Path, contents, cancellationToken);
    }

    public Task WriteAsync(IEnumerable<string> contents, CancellationToken cancellationToken = default)
    {
        LogFileOperation("Writing to File: {Path} [Module: {ModuleName}, Activity: {ActivityId}]", this);

        return System.IO.File.WriteAllLinesAsync(Path, contents, cancellationToken);
    }

    public async Task WriteAsync(ReadOnlyMemory<byte> contents, CancellationToken cancellationToken = default)
    {
        LogFileOperation("Writing to File: {Path} [Module: {ModuleName}, Activity: {ActivityId}]", this);

        var fileStream = System.IO.File.Create(Path);
        await using (fileStream.ConfigureAwait(false))
        {
            await fileStream.WriteAsync(contents, cancellationToken).ConfigureAwait(false);
        }
    }

    public async Task WriteAsync(Stream contents, CancellationToken cancellationToken = default)
    {
        LogFileOperation("Writing to File: {Path} [Module: {ModuleName}, Activity: {ActivityId}]", this);

        var fileStream = System.IO.File.Create(Path);
        await using (fileStream.ConfigureAwait(false))
        {
            if (contents.CanSeek)
            {
                contents.Position = 0;
            }

            await contents.CopyToAsync(fileStream, cancellationToken).ConfigureAwait(false);
        }
    }

    public Task AppendAsync(string contents, CancellationToken cancellationToken = default)
    {
        LogFileOperation("Appending to File: {Path} [Module: {ModuleName}, Activity: {ActivityId}]", this);

        return System.IO.File.AppendAllTextAsync(Path, contents, cancellationToken);
    }

    public Task AppendAsync(IEnumerable<string> contents, CancellationToken cancellationToken = default)
    {
        LogFileOperation("Appending to File: {Path} [Module: {ModuleName}, Activity: {ActivityId}]", this);

        return System.IO.File.AppendAllLinesAsync(Path, contents, cancellationToken);
    }

    /// <inheritdoc cref="FileSystemInfo.Exists"/>>
    public bool Exists => FileInfo.Exists;

    public bool Hidden => (FileInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;

    /// <inheritdoc cref="FileSystemInfo.Name"/>>
    public string Name => FileInfo.Name;

    /// <inheritdoc cref="System.IO.Path.GetFileNameWithoutExtension(System.ReadOnlySpan{char})"/>>
    public string NameWithoutExtension => System.IO.Path.GetFileNameWithoutExtension(this);

    /// <inheritdoc cref="FileInfo.Directory"/>>
    public Folder? Folder => FileInfo.Directory;

    /// <inheritdoc cref="FileSystemInfo.FullName"/>>
    public string Path => FileInfo.FullName;

    /// <summary>
    /// Gets the original path string that was used to construct this File instance.
    /// </summary>
    /// <remarks>
    /// Unlike <see cref="Path"/> which always returns the absolute path,
    /// this property preserves the original input (which may be relative).
    /// </remarks>
    public string OriginalPath { get; }

    public File Create()
    {
        LogFileOperation("Creating File: {Path} [Module: {ModuleName}, Activity: {ActivityId}]", this);

        var fileStream = System.IO.File.Create(Path);
        fileStream.Dispose();
        return this;
    }

    /// <summary>
    /// Asynchronously creates a new file at the current path.
    /// </summary>
    /// <returns>This file instance for method chaining.</returns>
    public async Task<File> CreateAsync()
    {
        LogFileOperation("Creating File: {Path} [Module: {ModuleName}, Activity: {ActivityId}]", this);

        var fileStream = System.IO.File.Create(Path);
        await fileStream.DisposeAsync().ConfigureAwait(false);
        return this;
    }

    /// <inheritdoc cref="FileSystemInfo.Attributes"/>>
    public FileAttributes Attributes
    {
        get { return FileInfo.Attributes; }
        set { FileInfo.Attributes = value; }
    }

    /// <inheritdoc cref="FileInfo.IsReadOnly"/>>
    public bool IsReadOnly => FileInfo.IsReadOnly;

    /// <inheritdoc cref="FileSystemInfo.CreationTime"/>>
    public DateTimeOffset CreationTime => FileInfo.CreationTime;

    public DateTimeOffset LastWriteTimeUtc => FileInfo.LastWriteTimeUtc;

    /// <inheritdoc cref="FileSystemInfo.Extension"/>>
    public string Extension => FileInfo.Extension;

    /// <inheritdoc cref="System.IO.FileInfo.Length"/>>
    public long Length => FileInfo.Length;

    /// <inheritdoc cref="FileInfo.Delete"/>>
    public void Delete()
    {
        LogFileOperation("Deleting File: {Path} [Module: {ModuleName}, Activity: {ActivityId}]", this);

        FileInfo.Delete();
    }

    /// <summary>
    /// Asynchronously deletes the file.
    /// </summary>
    /// <remarks>
    /// Uses thread pool offloading as no native async delete API exists in .NET.
    /// For true async I/O, consider using stream-based operations where available.
    /// </remarks>
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task DeleteAsync(CancellationToken cancellationToken = default)
    {
        LogFileOperation("Deleting File: {Path} [Module: {ModuleName}, Activity: {ActivityId}]", this);

        return Task.Run(() => FileInfo.Delete(), cancellationToken);
    }

    /// <inheritdoc cref="FileInfo.MoveTo(string)"/>>
    public File MoveTo(string path)
    {
        LogFileOperationWithDestination("Moving File: {Source} > {Destination} [Module: {ModuleName}, Activity: {ActivityId}]", this, path);

        FileInfo.MoveTo(path);
        return this;
    }

    /// <inheritdoc cref="FileInfo.MoveTo(string)"/>>
    public File MoveTo(Folder folder)
    {
        LogFileOperationWithDestination("Moving File: {Source} > {Destination} [Module: {ModuleName}, Activity: {ActivityId}]", this, folder);

        folder.Create();
        return MoveTo(System.IO.Path.Combine(folder.Path, Name));
    }

    /// <summary>
    /// Asynchronously moves the file to a new path.
    /// </summary>
    /// <remarks>
    /// Uses thread pool offloading as no native async move API exists in .NET.
    /// </remarks>
    /// <param name="path">The destination path.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>This file instance for method chaining.</returns>
    public Task<File> MoveToAsync(string path, CancellationToken cancellationToken = default)
    {
        LogFileOperationWithDestination("Moving File: {Source} > {Destination} [Module: {ModuleName}, Activity: {ActivityId}]", this, path);

        return Task.Run(() =>
        {
            FileInfo.MoveTo(path);
            return this;
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
    /// <returns>This file instance for method chaining.</returns>
    public async Task<File> MoveToAsync(Folder folder, CancellationToken cancellationToken = default)
    {
        LogFileOperationWithDestination("Moving File: {Source} > {Destination} [Module: {ModuleName}, Activity: {ActivityId}]", this, folder);

        await folder.CreateAsync().ConfigureAwait(false);
        return await MoveToAsync(System.IO.Path.Combine(folder.Path, Name), cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="FileInfo.CopyTo(string)"/>>
    public File CopyTo(string path)
    {
        LogFileOperationWithDestination("Copying File: {Source} > {Destination} [Module: {ModuleName}, Activity: {ActivityId}]", this, path);

        return FileInfo.CopyTo(path);
    }

    public File CopyTo(Folder folder)
    {
        LogFileOperationWithDestination("Copying File: {Source} > {Destination} [Module: {ModuleName}, Activity: {ActivityId}]", this, folder);

        folder.Create();
        return CopyTo(System.IO.Path.Combine(folder.Path, Name));
    }

    /// <summary>
    /// Asynchronously copies the file to a new path using stream-based copying.
    /// </summary>
    /// <param name="path">The destination path.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A new File instance representing the copied file.</returns>
    public async Task<File> CopyToAsync(string path, CancellationToken cancellationToken = default)
    {
        LogFileOperationWithDestination("Copying File: {Source} > {Destination} [Module: {ModuleName}, Activity: {ActivityId}]", this, path);

        await using var sourceStream = System.IO.File.OpenRead(Path);
        await using var destStream = System.IO.File.Create(path);
        await sourceStream.CopyToAsync(destStream, cancellationToken).ConfigureAwait(false);

        return new File(path);
    }

    /// <summary>
    /// Asynchronously copies the file to a folder using stream-based copying.
    /// </summary>
    /// <param name="folder">The destination folder.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A new File instance representing the copied file.</returns>
    public async Task<File> CopyToAsync(Folder folder, CancellationToken cancellationToken = default)
    {
        LogFileOperationWithDestination("Copying File: {Source} > {Destination} [Module: {ModuleName}, Activity: {ActivityId}]", this, folder);

        await folder.CreateAsync().ConfigureAwait(false);
        return await CopyToAsync(System.IO.Path.Combine(folder.Path, Name), cancellationToken).ConfigureAwait(false);
    }

    public static File GetNewTemporaryFilePath()
    {
        var path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName());

        LogFileOperation("Temporary File Path: {Path} [Module: {ModuleName}, Activity: {ActivityId}]", path);

        return path!;
    }

    public static implicit operator File?(string? path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return null;
        }

        return new FileInfo(path);
    }

    [return: NotNullIfNotNull("fileInfo")]
    public static implicit operator File?(FileInfo? fileInfo)
    {
        if (fileInfo == null)
        {
            return null;
        }

        return new File(fileInfo);
    }

    public static implicit operator string?(File? file)
    {
        return file?.Path;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Path;
    }

    /// <inheritdoc/>
    public bool Equals(File? other)
    {
        if (ReferenceEquals(null, other))
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

        return obj is File other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Path.GetHashCode();
    }

    public static bool operator ==(File? left, File? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(File? left, File? right)
    {
        return !Equals(left, right);
    }

    /// <summary>
    /// Logs a file operation with Activity context information.
    /// </summary>
    /// <remarks>
    /// Phase 2: Uses Activity.Current for context alongside AsyncLocal for backward compatibility.
    /// The log message includes the current module name and activity ID when available.
    /// </remarks>
    private static void LogFileOperation(string messageTemplate, object? arg1)
    {
        var moduleName = ModuleActivityTracing.GetCurrentModuleName() ?? "Unknown";
        var activityId = ModuleActivityTracing.GetCurrentActivityId();

        ModuleLogger.Current.LogInformation(messageTemplate, arg1, moduleName, activityId);
    }

    /// <summary>
    /// Logs a file operation with Activity context information for operations with source and destination.
    /// </summary>
    private static void LogFileOperationWithDestination(string messageTemplate, object? source, object? destination)
    {
        var moduleName = ModuleActivityTracing.GetCurrentModuleName() ?? "Unknown";
        var activityId = ModuleActivityTracing.GetCurrentActivityId();

        ModuleLogger.Current.LogInformation(messageTemplate, source, destination, moduleName, activityId);
    }
}
