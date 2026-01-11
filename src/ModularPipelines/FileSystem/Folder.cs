using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Microsoft.Extensions.Logging;
using ModularPipelines.JsonUtils;
using ModularPipelines.Logging;
using ModularPipelines.Tracing;

namespace ModularPipelines.FileSystem;

/// <summary>
/// Represents a folder in the file system with extended functionality for pipeline operations.
/// </summary>
public class Folder : IEquatable<Folder>
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

    public Folder(string path) : this(new DirectoryInfo(path), path, SystemFileSystemProvider.Instance)
    {
    }

    internal Folder(DirectoryInfo directoryInfo) : this(directoryInfo, directoryInfo.FullName, SystemFileSystemProvider.Instance)
    {
    }

    internal Folder(string path, IFileSystemProvider provider) : this(new DirectoryInfo(path), path, provider)
    {
    }

    private Folder(DirectoryInfo directoryInfo, string originalPath, IFileSystemProvider provider)
    {
        _directoryInfo = directoryInfo;
        OriginalPath = originalPath;
        _provider = provider;
    }

    public bool Exists => DirectoryInfo.Exists;

    public bool Hidden => (DirectoryInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;

    public string Name => DirectoryInfo.Name;

    [JsonConverter(typeof(FolderPathJsonConverter))]
    public Folder? Parent => DirectoryInfo.Parent;

    public string Path => DirectoryInfo.FullName;

    /// <summary>
    /// Gets the original path string that was used to construct this Folder instance.
    /// </summary>
    /// <remarks>
    /// Unlike <see cref="Path"/> which always returns the absolute path,
    /// this property preserves the original input (which may be relative).
    /// </remarks>
    public string OriginalPath { get; }

    public FileAttributes Attributes
    {
        get => DirectoryInfo.Attributes;
        set => DirectoryInfo.Attributes = value;
    }

    [JsonConverter(typeof(FolderPathJsonConverter))]
    public Folder Root
    {
        get
        {
            if (DirectoryInfo.Root.FullName == Path)
            {
                return this;
            }

            return DirectoryInfo.Root;
        }
    }

    public DateTimeOffset CreationTime => DirectoryInfo.CreationTime;

    public DateTimeOffset LastWriteTimeUtc => DirectoryInfo.LastWriteTimeUtc;

    public string Extension => DirectoryInfo.Extension;

    public Folder Create()
    {
        LogFolderOperation("Creating Folder: {Path} [Module: {ModuleName}, Activity: {ActivityId}]", this);

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
    public Task<Folder> CreateAsync(CancellationToken cancellationToken = default)
    {
        LogFolderOperation("Creating Folder: {Path} [Module: {ModuleName}, Activity: {ActivityId}]", this);

        return Task.Run(() =>
        {
            _provider.CreateDirectory(Path);
            return this;
        }, cancellationToken);
    }

    public void Delete()
    {
        LogFolderOperation("Deleting Folder: {Path} [Module: {ModuleName}, Activity: {ActivityId}]", this);

        _provider.DeleteDirectory(Path, recursive: true);
    }

    /// <summary>
    /// Asynchronously deletes the folder and all its contents.
    /// </summary>
    /// <remarks>
    /// Uses thread pool offloading as no native async delete API exists in .NET.
    /// </remarks>
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task DeleteAsync(CancellationToken cancellationToken = default)
    {
        LogFolderOperation("Deleting Folder: {Path} [Module: {ModuleName}, Activity: {ActivityId}]", this);

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
        LogFolderOperation("Cleaning Folder: {Path} [Module: {ModuleName}, Activity: {ActivityId}]", this);

        var errors = new List<Exception>();

        foreach (var directory in DirectoryInfo.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
        {
            try
            {
                if (removeReadOnlyAttribute)
                {
                    RemoveReadOnlyAttributeRecursively(directory);
                }

                directory.Delete(true);
            }
            catch (Exception ex) when (continueOnError)
            {
                LogFolderWarning(ex, "Failed to delete directory: {Path} [Module: {ModuleName}, Activity: {ActivityId}]", directory.FullName);
                errors.Add(ex);
            }
        }

        foreach (var file in DirectoryInfo.EnumerateFiles("*", SearchOption.TopDirectoryOnly))
        {
            try
            {
                if (removeReadOnlyAttribute && (file.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    file.Attributes &= ~FileAttributes.ReadOnly;
                }

                file.Delete();
            }
            catch (Exception ex) when (continueOnError)
            {
                LogFolderWarning(ex, "Failed to delete file: {Path} [Module: {ModuleName}, Activity: {ActivityId}]", file.FullName);
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
    /// <returns>A new <see cref="Folder"/> instance representing the copied folder.</returns>
    public Folder CopyTo(string targetPath)
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
    /// <returns>A new <see cref="Folder"/> instance representing the copied folder.</returns>
    public Folder CopyTo(string targetPath, bool preserveTimestamps)
    {
        LogFolderOperationWithDestination("Copying Folder: {Source} > {Destination} [Module: {ModuleName}, Activity: {ActivityId}]", this, targetPath);

        _provider.CreateDirectory(targetPath);

        // Copy all subdirectories first
        foreach (var dirPath in _provider.EnumerateDirectories(this, "*", SearchOption.AllDirectories))
        {
            var sourceDir = new DirectoryInfo(dirPath);
            var relativePath = _provider.GetRelativePath(this, dirPath);
            var newPath = _provider.Combine(targetPath, relativePath);
            _provider.CreateDirectory(newPath);

            var targetDir = new DirectoryInfo(newPath);

            // Preserve directory attributes
            targetDir.Attributes = sourceDir.Attributes;

            if (preserveTimestamps)
            {
                targetDir.CreationTimeUtc = sourceDir.CreationTimeUtc;
                targetDir.LastWriteTimeUtc = sourceDir.LastWriteTimeUtc;
                targetDir.LastAccessTimeUtc = sourceDir.LastAccessTimeUtc;
            }
        }

        // Copy all files
        foreach (var filePath in _provider.EnumerateFiles(this, "*", SearchOption.AllDirectories))
        {
            var sourceFile = new FileInfo(filePath);
            var relativePath = _provider.GetRelativePath(this, filePath);
            var newPath = _provider.Combine(targetPath, relativePath);
            _provider.CopyFile(filePath, newPath, overwrite: true);

            var targetFile = new FileInfo(newPath);

            // Preserve file attributes
            targetFile.Attributes = sourceFile.Attributes;

            if (preserveTimestamps)
            {
                targetFile.CreationTimeUtc = sourceFile.CreationTimeUtc;
                targetFile.LastWriteTimeUtc = sourceFile.LastWriteTimeUtc;
                targetFile.LastAccessTimeUtc = sourceFile.LastAccessTimeUtc;
            }
        }

        // Preserve root directory attributes and timestamps after all content is copied
        var targetRootDir = new DirectoryInfo(targetPath);
        targetRootDir.Attributes = DirectoryInfo.Attributes;

        if (preserveTimestamps)
        {
            targetRootDir.CreationTimeUtc = DirectoryInfo.CreationTimeUtc;
            targetRootDir.LastWriteTimeUtc = DirectoryInfo.LastWriteTimeUtc;
            targetRootDir.LastAccessTimeUtc = DirectoryInfo.LastAccessTimeUtc;
        }

        return new Folder(targetPath, _provider);
    }

    /// <summary>
    /// Asynchronously copies the folder and its contents to the specified target path using stream-based file copying.
    /// </summary>
    /// <param name="targetPath">The destination path for the copied folder.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A new <see cref="Folder"/> instance representing the copied folder.</returns>
    public Task<Folder> CopyToAsync(string targetPath, CancellationToken cancellationToken = default)
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
    /// <returns>A new <see cref="Folder"/> instance representing the copied folder.</returns>
    public async Task<Folder> CopyToAsync(string targetPath, bool preserveTimestamps, CancellationToken cancellationToken = default)
    {
        LogFolderOperationWithDestination("Copying Folder: {Source} > {Destination} [Module: {ModuleName}, Activity: {ActivityId}]", this, targetPath);

        _provider.CreateDirectory(targetPath);

        // Copy all subdirectories first
        foreach (var dirPath in _provider.EnumerateDirectories(this, "*", SearchOption.AllDirectories))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var sourceDir = new DirectoryInfo(dirPath);
            var relativePath = _provider.GetRelativePath(this, dirPath);
            var newPath = _provider.Combine(targetPath, relativePath);
            _provider.CreateDirectory(newPath);

            var targetDir = new DirectoryInfo(newPath);
            targetDir.Attributes = sourceDir.Attributes;

            if (preserveTimestamps)
            {
                targetDir.CreationTimeUtc = sourceDir.CreationTimeUtc;
                targetDir.LastWriteTimeUtc = sourceDir.LastWriteTimeUtc;
                targetDir.LastAccessTimeUtc = sourceDir.LastAccessTimeUtc;
            }
        }

        // Copy all files using async stream copying
        foreach (var filePath in _provider.EnumerateFiles(this, "*", SearchOption.AllDirectories))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var sourceFile = new FileInfo(filePath);
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

            var targetFile = new FileInfo(newPath);
            targetFile.Attributes = sourceFile.Attributes;

            if (preserveTimestamps)
            {
                targetFile.CreationTimeUtc = sourceFile.CreationTimeUtc;
                targetFile.LastWriteTimeUtc = sourceFile.LastWriteTimeUtc;
                targetFile.LastAccessTimeUtc = sourceFile.LastAccessTimeUtc;
            }
        }

        // Preserve root directory attributes and timestamps
        var targetRootDir = new DirectoryInfo(targetPath);
        targetRootDir.Attributes = DirectoryInfo.Attributes;

        if (preserveTimestamps)
        {
            targetRootDir.CreationTimeUtc = DirectoryInfo.CreationTimeUtc;
            targetRootDir.LastWriteTimeUtc = DirectoryInfo.LastWriteTimeUtc;
            targetRootDir.LastAccessTimeUtc = DirectoryInfo.LastAccessTimeUtc;
        }

        return new Folder(targetPath, _provider);
    }

    public Folder MoveTo(string path)
    {
        LogFolderOperationWithDestination("Moving Folder: {Source} > {Destination} [Module: {ModuleName}, Activity: {ActivityId}]", this, path);

        _provider.MoveDirectory(Path, path);
        return new Folder(path, _provider);
    }

    /// <summary>
    /// Asynchronously moves the folder to a new path.
    /// </summary>
    /// <remarks>
    /// Uses thread pool offloading as no native async move API exists in .NET.
    /// </remarks>
    /// <param name="path">The destination path.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A new Folder instance at the destination path.</returns>
    public Task<Folder> MoveToAsync(string path, CancellationToken cancellationToken = default)
    {
        LogFolderOperationWithDestination("Moving Folder: {Source} > {Destination} [Module: {ModuleName}, Activity: {ActivityId}]", this, path);

        return Task.Run(() =>
        {
            _provider.MoveDirectory(Path, path);
            return new Folder(path, _provider);
        }, cancellationToken);
    }

    public Folder GetFolder(string name)
    {
        var combinedPath = _provider.Combine(Path, name);

        LogFolderOperation("Getting Folder: {Path} [Module: {ModuleName}, Activity: {ActivityId}]", combinedPath);

        return new Folder(combinedPath, _provider);
    }

    public Folder CreateFolder(string name)
    {
        var folder = GetFolder(name).Create();

        LogFolderOperation("Creating Folder: {Path} [Module: {ModuleName}, Activity: {ActivityId}]", folder);

        return folder;
    }

    public File GetFile(string name)
    {
        var combinedPath = _provider.Combine(Path, name);

        LogFolderOperation("Getting File: {Path} [Module: {ModuleName}, Activity: {ActivityId}]", combinedPath);

        return new File(combinedPath, _provider);
    }

    public File CreateFile(string name)
    {
        return GetFile(name).Create();
    }

    public IEnumerable<Folder> GetFolders(Func<Folder, bool> predicate, [CallerArgumentExpression("predicate")] string predicateExpression = "") => GetFolders(predicate, _ => false, predicateExpression);

    public IEnumerable<File> GetFiles(Func<File, bool> predicate, [CallerArgumentExpression("predicate")] string predicateExpression = "") => GetFiles(predicate, _ => false, predicateExpression);

    public IEnumerable<Folder> GetFolders(Func<Folder, bool> predicate, Func<Folder, bool> exclusionFilters, [CallerArgumentExpression("predicate")] string predicateExpression = "")
    {
        LogFolderOperationWithExpression("Searching Folders in: {Path} > {Expression} [Module: {ModuleName}, Activity: {ActivityId}]", this, predicateExpression);

        return SafeWalk.EnumerateFolders(this, exclusionFilters)
            .Select(x => new Folder(x))
            .Distinct()
            .Where(predicate);
    }

    public IEnumerable<File> GetFiles(Func<File, bool> predicate, Func<Folder, bool> directoryExclusionFilters, [CallerArgumentExpression("predicate")] string predicateExpression = "")
    {
        LogFolderOperationWithExpression("Searching Files in: {Path} > {Expression} [Module: {ModuleName}, Activity: {ActivityId}]", this, predicateExpression);

        return SafeWalk.EnumerateFiles(this, directoryExclusionFilters)
            .Select(x => new File(x))
            .Distinct()
            .Where(predicate);
    }

    public IEnumerable<File> GetFiles(string globPattern)
    {
        LogFolderOperationWithExpression("Searching Files in: {Path} > {Glob} [Module: {ModuleName}, Activity: {ActivityId}]", this, globPattern);

        return new Matcher(StringComparison.OrdinalIgnoreCase)
            .AddInclude(globPattern)
            .Execute(new DirectoryInfoWrapper(DirectoryInfo))
            .Files
            .Select(x => new File(System.IO.Path.Combine(this, x.Path)))
            .Distinct();
    }

    public File? FindFile(Func<File, bool> predicate, [CallerArgumentExpression("predicate")] string predicateExpression = "") => FindFile(predicate, _ => false, predicateExpression);

    public Folder? FindFolder(Func<Folder, bool> predicate, [CallerArgumentExpression("predicate")] string predicateExpression = "") => FindFolder(predicate, _ => false, predicateExpression);

    public File? FindFile(Func<File, bool> predicate, Func<Folder, bool> directoryExclusionFilters, [CallerArgumentExpression("predicate")] string predicateExpression = "") => GetFiles(predicate, directoryExclusionFilters, predicateExpression).FirstOrDefault();

    public Folder? FindFolder(Func<Folder, bool> predicate, Func<Folder, bool> directoryExclusionFilters, [CallerArgumentExpression("predicate")] string predicateExpression = "") => GetFolders(predicate, directoryExclusionFilters, predicateExpression).FirstOrDefault();

    public IEnumerable<File> ListFiles()
    {
        return DirectoryInfo.EnumerateFiles("*", SearchOption.TopDirectoryOnly)
            .Select(x => new File(x))
            .Distinct();
    }

    public IEnumerable<Folder> ListFolders()
    {
        return DirectoryInfo.EnumerateDirectories("*", SearchOption.TopDirectoryOnly)
            .Select(x => new Folder(x))
            .Distinct();
    }

    public static Folder CreateTemporaryFolder()
    {
        var provider = SystemFileSystemProvider.Instance;
        var tempDirectory = provider.Combine(provider.GetTempPath(), provider.GetRandomFileName().Replace(".", string.Empty));
        provider.CreateDirectory(tempDirectory);

        LogFolderOperation("Creating Temporary Folder: {Path} [Module: {ModuleName}, Activity: {ActivityId}]", tempDirectory);

        return tempDirectory!;
    }

    public static implicit operator Folder?(string? path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return null;
        }

        return new DirectoryInfo(path);
    }

    [return: NotNullIfNotNull("directoryInfo")]
    public static implicit operator Folder?(DirectoryInfo? directoryInfo)
    {
        if (directoryInfo == null)
        {
            return null;
        }

        return new Folder(directoryInfo);
    }

    public static implicit operator string?(Folder? folder)
    {
        return folder?.Path;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Path;
    }

    /// <inheritdoc/>
    public bool Equals(Folder? other)
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

        return obj is Folder other && Equals(other);
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

    public static bool operator ==(Folder? left, Folder? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Folder? left, Folder? right)
    {
        return !Equals(left, right);
    }

    private static void RemoveReadOnlyAttributeRecursively(DirectoryInfo directory)
    {
        if ((directory.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
        {
            directory.Attributes &= ~FileAttributes.ReadOnly;
        }

        foreach (var file in directory.EnumerateFiles("*", SearchOption.TopDirectoryOnly))
        {
            if ((file.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                file.Attributes &= ~FileAttributes.ReadOnly;
            }
        }

        foreach (var subDirectory in directory.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
        {
            RemoveReadOnlyAttributeRecursively(subDirectory);
        }
    }

    /// <summary>
    /// Logs a folder operation with Activity context information.
    /// </summary>
    /// <remarks>
    /// Phase 2: Uses Activity.Current for context alongside AsyncLocal for backward compatibility.
    /// The log message includes the current module name and activity ID when available.
    /// </remarks>
    private static void LogFolderOperation(string messageTemplate, object? arg1)
    {
        var moduleName = ModuleActivityTracing.GetCurrentModuleName() ?? "Unknown";
        var activityId = ModuleActivityTracing.GetCurrentActivityId();

        ModuleLogger.Current.LogInformation(messageTemplate, arg1, moduleName, activityId);
    }

    /// <summary>
    /// Logs a folder operation with Activity context information for operations with source and destination.
    /// </summary>
    private static void LogFolderOperationWithDestination(string messageTemplate, object? source, object? destination)
    {
        var moduleName = ModuleActivityTracing.GetCurrentModuleName() ?? "Unknown";
        var activityId = ModuleActivityTracing.GetCurrentActivityId();

        ModuleLogger.Current.LogInformation(messageTemplate, source, destination, moduleName, activityId);
    }

    /// <summary>
    /// Logs a folder operation with Activity context information for operations with path and expression/glob.
    /// </summary>
    private static void LogFolderOperationWithExpression(string messageTemplate, object? path, object? expression)
    {
        var moduleName = ModuleActivityTracing.GetCurrentModuleName() ?? "Unknown";
        var activityId = ModuleActivityTracing.GetCurrentActivityId();

        ModuleLogger.Current.LogInformation(messageTemplate, path, expression, moduleName, activityId);
    }

    /// <summary>
    /// Logs a folder warning with Activity context information.
    /// </summary>
    private static void LogFolderWarning(Exception ex, string messageTemplate, object? arg1)
    {
        var moduleName = ModuleActivityTracing.GetCurrentModuleName() ?? "Unknown";
        var activityId = ModuleActivityTracing.GetCurrentActivityId();

        ModuleLogger.Current.LogWarning(ex, messageTemplate, arg1, moduleName, activityId);
    }
}
