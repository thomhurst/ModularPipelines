using Microsoft.Extensions.FileSystemGlobbing;
using ModularPipelines.Context.Domains.Files;
using ModularPipelines.FileSystem;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Context.Domains.Implementations;

/// <summary>
/// Provides file system operations with rich File and Folder return types.
/// </summary>
internal class FilesContext(
    IFileSystemProvider fileSystemProvider,
    PipelineWorkingDirectory workingDirectory,
    IZipContext zip) : IFilesContext
{
    private readonly IFileSystemProvider _fileSystemProvider = fileSystemProvider;
    private readonly PipelineWorkingDirectory _workingDirectory = workingDirectory;

    /// <inheritdoc />
    public File GetFile(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        return new File(_workingDirectory.ResolvePath(path), _fileSystemProvider);
    }

    /// <inheritdoc />
    public Folder GetFolder(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        return new Folder(_workingDirectory.ResolvePath(path), _fileSystemProvider);
    }

    /// <inheritdoc />
    public Folder GetFolder(System.Environment.SpecialFolder specialFolder) =>
        new(System.Environment.GetFolderPath(specialFolder), _fileSystemProvider);

    /// <inheritdoc />
    public IEnumerable<File> Glob(string pattern) =>
        GetFolder(_workingDirectory.Path).GetFiles(pattern);

    /// <inheritdoc />
    public IEnumerable<Folder> GlobFolders(string pattern)
    {
        var currentDirectory = _workingDirectory.Path;
        var matcher = new Matcher(StringComparison.OrdinalIgnoreCase)
            .AddInclude(pattern);

        return _fileSystemProvider
            .EnumerateDirectories(currentDirectory, "*", SearchOption.AllDirectories)
            .Where(path => matcher.Match(
                _fileSystemProvider.GetRelativePath(currentDirectory, path)).HasMatches)
            .Select(GetFolder)
            .Distinct();
    }

    /// <inheritdoc />
    public Task<string> ReadAsync(string path, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        return _fileSystemProvider.ReadAllTextAsync(_workingDirectory.ResolvePath(path), cancellationToken);
    }

    /// <inheritdoc />
    public Task WriteAsync(string path, string content, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        return _fileSystemProvider.WriteAllTextAsync(_workingDirectory.ResolvePath(path), content, cancellationToken);
    }

    /// <inheritdoc />
    public Task<bool> ExistsAsync(string path, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (string.IsNullOrWhiteSpace(path))
        {
            return Task.FromResult(false);
        }

        var resolvedPath = _workingDirectory.ResolvePath(path);
        return Task.FromResult(
            _fileSystemProvider.FileExists(resolvedPath)
            || _fileSystemProvider.DirectoryExists(resolvedPath));
    }

    /// <inheritdoc />
    public IZipContext Zip { get; } = zip;
}
