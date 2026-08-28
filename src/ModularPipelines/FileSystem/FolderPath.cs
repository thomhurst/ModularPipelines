using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Microsoft.Extensions.Logging;
using ModularPipelines.Logging;
using ModularPipelines.Serialization;

namespace ModularPipelines.FileSystem;

/// <summary>
/// Represents a folder in the file system with extended functionality for pipeline operations.
/// </summary>
[JsonConverter(typeof(FolderPathJsonConverter))]
public class FolderPath : IEquatable<FolderPath>
{
    [JsonIgnore]
    private readonly DirectoryInfo _directoryInfo;

    private readonly IFileSystemProvider _provider;

    private DirectoryInfo DirectoryInfo
    {
        get
        {
            _directoryInfo.Refresh();
            return _directoryInfo;
        }
    }

    public FolderPath(string path) : this(new DirectoryInfo(path), path, SystemFileSystemProvider.Instance)
    {
    }

    internal FolderPath(DirectoryInfo directoryInfo) : this(directoryInfo, directoryInfo.FullName, SystemFileSystemProvider.Instance)
    {
    }

    internal FolderPath(string path, IFileSystemProvider provider) : this(new DirectoryInfo(path), path, provider)
    {
    }

    private FolderPath(DirectoryInfo directoryInfo, string originalPath, IFileSystemProvider provider)
    {
        _directoryInfo = directoryInfo;
        OriginalPath = originalPath;
        _provider = provider;
    }

    public bool Exists => _provider.DirectoryExists(Path);

    public bool Hidden => (GetPhysicalDirectoryInfo().Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;

    public string Name => _directoryInfo.Name;

    [JsonConverter(typeof(FolderPathJsonConverter))]
    public FolderPath? Parent => System.IO.Path.GetDirectoryName(
        Path.TrimEnd(
            System.IO.Path.DirectorySeparatorChar,
            System.IO.Path.AltDirectorySeparatorChar)) is { } parent
        ? new FolderPath(parent, _provider)
        : null;

    public string Path => _directoryInfo.FullName;

    /// <summary>
    /// Gets the original path string that was used to construct this FolderPath instance.
    /// </summary>
    /// <remarks>
    /// Unlike <see cref="Path"/> which always returns the absolute path,
    /// this property preserves the original input (which may be relative).
    /// </remarks>
    public string OriginalPath { get; }

    public FileAttributes Attributes
    {
        get => GetPhysicalDirectoryInfo().Attributes;
        set => GetPhysicalDirectoryInfo().Attributes = value;
    }

    [JsonConverter(typeof(FolderPathJsonConverter))]
    public FolderPath Root
    {
        get
        {
            var rootPath = System.IO.Path.GetPathRoot(Path)!;
            if (rootPath == Path)
            {
                return this;
            }

            return new FolderPath(rootPath, _provider);
        }
    }

    public DateTimeOffset CreationTime => GetPhysicalDirectoryInfo().CreationTime;

    public DateTimeOffset LastWriteTimeUtc => GetPhysicalDirectoryInfo().LastWriteTimeUtc;

    public string Extension => System.IO.Path.GetExtension(Path);

    public FolderPath Create()
    {
        LogFolderOperation("Creating Folder: {Path}", this);

        _provider.CreateDirectory(Path);
        return this;
    }

    /// <summary>
    /// Asynchronously creates the folder if it does not exist.
    /// </summary>
    /// <remarks>
    /// Uses thread pool offloading as no native async directory creation API exists in .NET.
    /// </remarks>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>This folder instance for method chaining.</returns>
    public Task<FolderPath> CreateAsync(CancellationToken cancellationToken = default)
    {
        LogFolderOperation("Creating Folder: {Path}", this);

        return Task.Run(() =>
        {
            _provider.CreateDirectory(Path);
            return this;
        }, cancellationToken);
    }

    public void Delete()
    {
        LogFolderOperation("Deleting Folder: {Path}", this);

        _provider.DeleteDirectory(Path, recursive: true);
    }

    /// <summary>
    /// Asynchronously deletes the folder and all its contents.
    /// </summary>
    /// <remarks>
    /// Uses thread pool offloading as no native async delete API exists in .NET.
    /// </remarks>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task that completes when the folder has been deleted.</returns>
    public Task DeleteAsync(CancellationToken cancellationToken = default)
    {
        LogFolderOperation("Deleting Folder: {Path}", this);

        return Task.Run(() => _provider.DeleteDirectory(Path, recursive: true), cancellationToken);
    }

    /// <summary>
    /// Removes all files and subdirectories within the folder.
    /// </summary>
    /// <remarks>
    /// This method preserves backward compatibility by not removing read-only attributes and failing on first error.
    /// Use <see cref="Clean(bool, bool)"/> for more control over error handling.
    /// </remarks>
    public void Clean()
    {
        Clean(removeReadOnlyAttribute: false, continueOnError: false);
    }

    /// <summary>
    /// Removes all files and subdirectories within the folder.
    /// </summary>
    /// <param name="removeReadOnlyAttribute">
    /// When true, removes the read-only attribute from files and directories before deletion.
    /// This helps handle read-only items that would otherwise fail to delete.
    /// </param>
    public void Clean(bool removeReadOnlyAttribute)
    {
        Clean(removeReadOnlyAttribute, continueOnError: false);
    }

    /// <summary>
    /// Removes all files and subdirectories within the folder.
    /// </summary>
    /// <param name="removeReadOnlyAttribute">
    /// When true, removes the read-only attribute from files and directories before deletion.
    /// This helps handle read-only items that would otherwise fail to delete.
    /// </param>
    /// <param name="continueOnError">
    /// When true, continues deleting remaining items even if some deletions fail.
    /// Failed deletions are logged and aggregated into a single exception at the end.
    /// When false, the first error encountered will stop the operation and throw immediately.
    /// </param>
    /// <exception cref="AggregateException">
    /// Thrown when <paramref name="continueOnError"/> is true and one or more deletions failed.
    /// Contains all individual exceptions encountered during the operation.
    /// </exception>
    public void Clean(bool removeReadOnlyAttribute, bool continueOnError)
    {
        LogFolderOperation("Cleaning Folder: {Path}", this);

        if (removeReadOnlyAttribute)
        {
            EnsurePhysicalMetadataSupported();
        }

        var errors = new List<Exception>();

        foreach (var directoryPath in _provider
                     .EnumerateDirectories(Path, "*", SearchOption.TopDirectoryOnly)
                     .ToArray())
        {
            try
            {
                if (removeReadOnlyAttribute)
                {
                    RemoveReadOnlyAttributeRecursively(new DirectoryInfo(directoryPath));
                }

                _provider.DeleteDirectory(directoryPath, recursive: true);
            }
            catch (Exception ex) when (continueOnError)
            {
                LogFolderWarning(ex, "Failed to delete directory: {Path}", directoryPath);
                errors.Add(ex);
            }
        }

        foreach (var filePath in _provider
                     .EnumerateFiles(Path, "*", SearchOption.TopDirectoryOnly)
                     .ToArray())
        {
            try
            {
                var file = new FileInfo(filePath);
                if (removeReadOnlyAttribute)
                {
                    RemoveReadOnlyAttribute(file);
                }

                _provider.DeleteFile(filePath);
            }
            catch (Exception ex) when (continueOnError)
            {
                LogFolderWarning(ex, "Failed to delete file: {Path}", filePath);
                errors.Add(ex);
            }
        }

        if (errors.Count > 0)
        {
            throw new AggregateException($"Failed to delete {errors.Count} item(s) in folder {Path}", errors);
        }
    }

    /// <summary>
    /// Copies the folder and its contents to the specified target path.
    /// </summary>
    /// <param name="targetPath">The destination path for the copied folder.</param>
    /// <returns>A new <see cref="FolderPath"/> instance representing the copied folder.</returns>
    public FolderPath CopyTo(string targetPath)
    {
        return CopyTo(targetPath, preserveTimestamps: false);
    }

    /// <summary>
    /// Copies the folder and its contents to the specified target path.
    /// </summary>
    /// <param name="targetPath">The destination path for the copied folder.</param>
    /// <param name="preserveTimestamps">
    /// When true, preserves CreationTimeUtc, LastWriteTimeUtc, and LastAccessTimeUtc
    /// for all files and directories.
    /// </param>
    /// <returns>A new <see cref="FolderPath"/> instance representing the copied folder.</returns>
    public FolderPath CopyTo(string targetPath, bool preserveTimestamps)
    {
        LogFolderOperationWithDestination("Copying Folder: {Source} > {Destination}", this, targetPath);
        var copyPhysicalMetadata = ReferenceEquals(_provider, SystemFileSystemProvider.Instance);
        if (preserveTimestamps && !copyPhysicalMetadata)
        {
            EnsurePhysicalMetadataSupported();
        }

        _provider.CreateDirectory(targetPath);

        // Copy all subdirectories first
        foreach (var dirPath in _provider.EnumerateDirectories(this, "*", SearchOption.AllDirectories))
        {
            var relativePath = _provider.GetRelativePath(this, dirPath);
            var newPath = _provider.Combine(targetPath, relativePath);
            _provider.CreateDirectory(newPath);

            if (copyPhysicalMetadata)
            {
                CopyDirectoryMetadata(dirPath, newPath, preserveTimestamps);
            }
        }

        // Copy all files
        foreach (var filePath in _provider.EnumerateFiles(this, "*", SearchOption.AllDirectories))
        {
            var relativePath = _provider.GetRelativePath(this, filePath);
            var newPath = _provider.Combine(targetPath, relativePath);
            _provider.CopyFile(filePath, newPath, overwrite: true);

            if (copyPhysicalMetadata)
            {
                CopyFileMetadata(filePath, newPath, preserveTimestamps);
            }
        }

        if (copyPhysicalMetadata)
        {
            CopyDirectoryMetadata(Path, targetPath, preserveTimestamps);
        }

        return new FolderPath(targetPath, _provider);
    }

    /// <summary>
    /// Asynchronously copies the folder and its contents to the specified target path using stream-based file copying.
    /// </summary>
    /// <param name="targetPath">The destination path for the copied folder.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A new <see cref="FolderPath"/> instance representing the copied folder.</returns>
    public Task<FolderPath> CopyToAsync(string targetPath, CancellationToken cancellationToken = default)
    {
        return CopyToAsync(targetPath, preserveTimestamps: false, cancellationToken);
    }

    /// <summary>
    /// Asynchronously copies the folder and its contents to the specified target path using stream-based file copying.
    /// </summary>
    /// <param name="targetPath">The destination path for the copied folder.</param>
    /// <param name="preserveTimestamps">
    /// When true, preserves CreationTimeUtc, LastWriteTimeUtc, and LastAccessTimeUtc
    /// for all files and directories.
    /// </param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A new <see cref="FolderPath"/> instance representing the copied folder.</returns>
    public async Task<FolderPath> CopyToAsync(string targetPath, bool preserveTimestamps, CancellationToken cancellationToken = default)
    {
        LogFolderOperationWithDestination("Copying Folder: {Source} > {Destination}", this, targetPath);
        var copyPhysicalMetadata = ReferenceEquals(_provider, SystemFileSystemProvider.Instance);
        if (preserveTimestamps && !copyPhysicalMetadata)
        {
            EnsurePhysicalMetadataSupported();
        }

        _provider.CreateDirectory(targetPath);

        // Copy all subdirectories first
        foreach (var dirPath in _provider.EnumerateDirectories(this, "*", SearchOption.AllDirectories))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var relativePath = _provider.GetRelativePath(this, dirPath);
            var newPath = _provider.Combine(targetPath, relativePath);
            _provider.CreateDirectory(newPath);

            if (copyPhysicalMetadata)
            {
                CopyDirectoryMetadata(dirPath, newPath, preserveTimestamps);
            }
        }

        // Copy all files using async stream copying
        foreach (var filePath in _provider.EnumerateFiles(this, "*", SearchOption.AllDirectories))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var relativePath = _provider.GetRelativePath(this, filePath);
            var newPath = _provider.Combine(targetPath, relativePath);

            var sourceStream = _provider.OpenRead(filePath);
            await using (sourceStream.ConfigureAwait(false))
            {
                var destStream = _provider.Create(newPath);
                await using (destStream.ConfigureAwait(false))
                {
                    await sourceStream.CopyToAsync(destStream, cancellationToken).ConfigureAwait(false);
                }
            }

            if (copyPhysicalMetadata)
            {
                CopyFileMetadata(filePath, newPath, preserveTimestamps);
            }
        }

        if (copyPhysicalMetadata)
        {
            CopyDirectoryMetadata(Path, targetPath, preserveTimestamps);
        }

        return new FolderPath(targetPath, _provider);
    }

    public FolderPath MoveTo(string path)
    {
        LogFolderOperationWithDestination("Moving Folder: {Source} > {Destination}", this, path);

        _provider.MoveDirectory(Path, path);
        return new FolderPath(path, _provider);
    }

    /// <summary>
    /// Asynchronously moves the folder to a new path.
    /// </summary>
    /// <remarks>
    /// Uses thread pool offloading as no native async move API exists in .NET.
    /// </remarks>
    /// <param name="path">The destination path.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A new FolderPath instance at the destination path.</returns>
    public Task<FolderPath> MoveToAsync(string path, CancellationToken cancellationToken = default)
    {
        LogFolderOperationWithDestination("Moving Folder: {Source} > {Destination}", this, path);

        return Task.Run(() =>
        {
            _provider.MoveDirectory(Path, path);
            return new FolderPath(path, _provider);
        }, cancellationToken);
    }

    public FolderPath GetFolder(string name)
    {
        var combinedPath = _provider.Combine(Path, name);

        LogFolderOperation("Getting Folder: {Path}", combinedPath);

        return new FolderPath(combinedPath, _provider);
    }

    public FolderPath CreateFolder(string name)
    {
        var folder = GetFolder(name).Create();

        LogFolderOperation("Creating Folder: {Path}", folder);

        return folder;
    }

    public FilePath GetFile(string name)
    {
        return new FilePath(_provider.Combine(Path, name), _provider);
    }

    public FilePath CreateFile(string name)
    {
        return GetFile(name).Create();
    }

    public IEnumerable<FolderPath> GetFolders(Func<FolderPath, bool> predicate, [CallerArgumentExpression("predicate")] string predicateExpression = "") => GetFolders(predicate, _ => false, predicateExpression);

    public IEnumerable<FilePath> GetFiles(Func<FilePath, bool> predicate, [CallerArgumentExpression("predicate")] string predicateExpression = "") => GetFiles(predicate, _ => false, predicateExpression);

    public IEnumerable<FolderPath> GetFolders(Func<FolderPath, bool> predicate, Func<FolderPath, bool> exclusionFilters, [CallerArgumentExpression("predicate")] string predicateExpression = "")
    {
        LogFolderOperationWithExpression("Searching Folders in: {Path} > {Expression}", this, predicateExpression);

        return EnumerateFolders(exclusionFilters)
            .Distinct()
            .Where(predicate);
    }

    public IEnumerable<FilePath> GetFiles(Func<FilePath, bool> predicate, Func<FolderPath, bool> directoryExclusionFilters, [CallerArgumentExpression("predicate")] string predicateExpression = "")
    {
        LogFolderOperationWithExpression("Searching Files in: {Path} > {Expression}", this, predicateExpression);

        return EnumerateFiles(directoryExclusionFilters)
            .Distinct()
            .Where(predicate);
    }

    public IEnumerable<FilePath> GetFiles(string globPattern)
    {
        LogFolderOperationWithExpression("Searching Files in: {Path} > {Glob}", this, globPattern);

        var matcher = new Matcher(StringComparison.OrdinalIgnoreCase)
            .AddInclude(globPattern);
        return _provider.EnumerateFiles(Path, "*", SearchOption.AllDirectories)
            .Where(path => matcher.Match(_provider.GetRelativePath(Path, path)).HasMatches)
            .Select(path => new FilePath(path, _provider))
            .Distinct();
    }

    public FilePath? FindFile(Func<FilePath, bool> predicate, [CallerArgumentExpression("predicate")] string predicateExpression = "") => FindFile(predicate, _ => false, predicateExpression);

    public FolderPath? FindFolder(Func<FolderPath, bool> predicate, [CallerArgumentExpression("predicate")] string predicateExpression = "") => FindFolder(predicate, _ => false, predicateExpression);

    public FilePath? FindFile(Func<FilePath, bool> predicate, Func<FolderPath, bool> directoryExclusionFilters, [CallerArgumentExpression("predicate")] string predicateExpression = "") => GetFiles(predicate, directoryExclusionFilters, predicateExpression).FirstOrDefault();

    public FolderPath? FindFolder(Func<FolderPath, bool> predicate, Func<FolderPath, bool> directoryExclusionFilters, [CallerArgumentExpression("predicate")] string predicateExpression = "") => GetFolders(predicate, directoryExclusionFilters, predicateExpression).FirstOrDefault();

    public IEnumerable<FilePath> ListFiles()
    {
        return _provider.EnumerateFiles(Path, "*", SearchOption.TopDirectoryOnly)
            .Select(path => new FilePath(path, _provider))
            .Distinct();
    }

    public IEnumerable<FolderPath> ListFolders()
    {
        return _provider.EnumerateDirectories(Path, "*", SearchOption.TopDirectoryOnly)
            .Select(path => new FolderPath(path, _provider))
            .Distinct();
    }

    public static FolderPath CreateTemporaryFolder() => CreateTemporaryFolder(SystemFileSystemProvider.Instance);

    /// <summary>
    /// Creates a temporary folder using the specified file system provider.
    /// </summary>
    /// <param name="provider">The provider used to create and access the folder.</param>
    /// <returns>The created temporary folder.</returns>
    public static FolderPath CreateTemporaryFolder(IFileSystemProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider);
        var tempDirectory = provider.Combine(provider.GetTempPath(), provider.GetRandomFileName().Replace(".", string.Empty));
        provider.CreateDirectory(tempDirectory);

        LogFolderOperation("Creating Temporary Folder: {Path}", tempDirectory);

        return new FolderPath(tempDirectory, provider);
    }

    public static implicit operator FolderPath?(string? path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return null;
        }

        return new DirectoryInfo(path);
    }

    [return: NotNullIfNotNull("directoryInfo")]
    public static implicit operator FolderPath?(DirectoryInfo? directoryInfo)
    {
        if (directoryInfo == null)
        {
            return null;
        }

        return new FolderPath(directoryInfo);
    }

    [return: NotNullIfNotNull(parameterName: "folder")]
    public static implicit operator string?(FolderPath? folder)
    {
        return folder?.Path;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Path;
    }

    /// <inheritdoc/>
    public bool Equals(FolderPath? other)
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

        return obj is FolderPath other && Equals(other);
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

    public static bool operator ==(FolderPath? left, FolderPath? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(FolderPath? left, FolderPath? right)
    {
        return !Equals(left, right);
    }

    private IEnumerable<FolderPath> EnumerateFolders(Func<FolderPath, bool> exclusionFilter)
    {
        if (ReferenceEquals(_provider, SystemFileSystemProvider.Instance))
        {
            return SafeWalk.EnumerateFolders(this, exclusionFilter)
                .Select(path => new FolderPath(path, _provider));
        }

        return _provider.EnumerateDirectories(Path, "*", SearchOption.AllDirectories)
            .Where(path => !IsExcludedByDirectoryFilter(path, includeEntry: true, exclusionFilter))
            .Select(path => new FolderPath(path, _provider));
    }

    private IEnumerable<FilePath> EnumerateFiles(Func<FolderPath, bool> exclusionFilter)
    {
        if (ReferenceEquals(_provider, SystemFileSystemProvider.Instance))
        {
            return SafeWalk.EnumerateFiles(this, exclusionFilter)
                .Select(path => new FilePath(path, _provider));
        }

        return _provider.EnumerateFiles(Path, "*", SearchOption.AllDirectories)
            .Where(path => !IsExcludedByDirectoryFilter(path, includeEntry: false, exclusionFilter))
            .Select(path => new FilePath(path, _provider));
    }

    private static void CopyDirectoryMetadata(
        string sourcePath,
        string targetPath,
        bool preserveTimestamps)
    {
        var source = new DirectoryInfo(sourcePath);
        var target = new DirectoryInfo(targetPath)
        {
            Attributes = source.Attributes,
        };

        if (preserveTimestamps)
        {
            target.CreationTimeUtc = source.CreationTimeUtc;
            target.LastWriteTimeUtc = source.LastWriteTimeUtc;
            target.LastAccessTimeUtc = source.LastAccessTimeUtc;
        }
    }

    private static void CopyFileMetadata(
        string sourcePath,
        string targetPath,
        bool preserveTimestamps)
    {
        var source = new FileInfo(sourcePath);
        var target = new FileInfo(targetPath)
        {
            Attributes = source.Attributes,
        };

        if (preserveTimestamps)
        {
            target.CreationTimeUtc = source.CreationTimeUtc;
            target.LastWriteTimeUtc = source.LastWriteTimeUtc;
            target.LastAccessTimeUtc = source.LastAccessTimeUtc;
        }
    }

    private bool IsExcludedByDirectoryFilter(
        string entryPath,
        bool includeEntry,
        Func<FolderPath, bool> exclusionFilter)
    {
        var current = includeEntry
            ? entryPath
            : System.IO.Path.GetDirectoryName(entryPath);
        while (current is not null)
        {
            if (_provider.GetRelativePath(Path, current) == ".")
            {
                break;
            }

            if (exclusionFilter(new FolderPath(current, _provider)))
            {
                return true;
            }

            current = System.IO.Path.GetDirectoryName(
                current.TrimEnd(
                    System.IO.Path.DirectorySeparatorChar,
                    System.IO.Path.AltDirectorySeparatorChar));
        }

        return false;
    }

    private DirectoryInfo GetPhysicalDirectoryInfo()
    {
        EnsurePhysicalMetadataSupported();
        return DirectoryInfo;
    }

    private void EnsurePhysicalMetadataSupported()
    {
        if (!ReferenceEquals(_provider, SystemFileSystemProvider.Instance))
        {
            throw new NotSupportedException(
                "Folder metadata is unavailable through the configured IFileSystemProvider.");
        }
    }

    private static void RemoveReadOnlyAttributeRecursively(DirectoryInfo directory)
    {
        if ((directory.Attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint)
        {
            return;
        }

        if ((directory.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
        {
            directory.Attributes &= ~FileAttributes.ReadOnly;
        }

        foreach (var file in directory.EnumerateFiles("*", SearchOption.TopDirectoryOnly))
        {
            RemoveReadOnlyAttribute(file);
        }

        foreach (var subDirectory in directory.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
        {
            RemoveReadOnlyAttributeRecursively(subDirectory);
        }
    }

    private static void RemoveReadOnlyAttribute(FileInfo file)
    {
        var attributes = file.Attributes;
        if ((attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint)
        {
            return;
        }

        if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
        {
            file.Attributes = attributes & ~FileAttributes.ReadOnly;
        }
    }

    /// <summary>
    /// Logs a folder operation.
    /// </summary>
    private static void LogFolderOperation(string messageTemplate, object? arg1)
    {
        ModuleLogger.Current.LogInformation(messageTemplate, arg1);
    }

    /// <summary>
    /// Logs a folder operation for operations with source and destination.
    /// </summary>
    private static void LogFolderOperationWithDestination(string messageTemplate, object? source, object? destination)
    {
        ModuleLogger.Current.LogInformation(messageTemplate, source, destination);
    }

    /// <summary>
    /// Logs a folder operation for operations with path and expression/glob.
    /// </summary>
    private static void LogFolderOperationWithExpression(string messageTemplate, object? path, object? expression)
    {
        ModuleLogger.Current.LogInformation(messageTemplate, path, expression);
    }

    /// <summary>
    /// Logs a folder warning.
    /// </summary>
    private static void LogFolderWarning(Exception ex, string messageTemplate, object? arg1)
    {
        ModuleLogger.Current.LogWarning(ex, messageTemplate, arg1);
    }
}
