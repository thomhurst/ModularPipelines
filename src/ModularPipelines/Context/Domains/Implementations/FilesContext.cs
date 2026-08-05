using Microsoft.Extensions.FileSystemGlobbing;
using ModularPipelines.Context.Domains.Files;
using ModularPipelines.FileSystem;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Context.Domains.Implementations;

/// <summary>
/// Provides file system operations with rich File and Folder return types.
/// </summary>
internal class FilesContext(
    IFileSystemContext fileSystemContext,
    IFileSystemProvider fileSystemProvider,
    PipelineWorkingDirectory workingDirectory,
    IZipContext zip,
    IChecksumContext checksum) : IFilesContext
{
    private readonly IFileSystemContext _fileSystemContext = fileSystemContext;
    private readonly IFileSystemProvider _fileSystemProvider = fileSystemProvider;
    private readonly PipelineWorkingDirectory _workingDirectory = workingDirectory;

    /// <inheritdoc />
    public File GetFile(string path) => _fileSystemContext.GetFile(path);

    /// <inheritdoc />
    public Folder GetFolder(string path) => _fileSystemContext.GetFolder(path);

    /// <inheritdoc />
    public Folder GetFolder(System.Environment.SpecialFolder specialFolder) => _fileSystemContext.GetFolder(specialFolder);

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
            .Select(_fileSystemContext.GetFolder)
            .Distinct();
    }

    /// <inheritdoc />
    public Task<string> ReadAsync(string path, CancellationToken cancellationToken = default)
        => _fileSystemProvider.ReadAllTextAsync(_workingDirectory.ResolvePath(path), cancellationToken);

    /// <inheritdoc />
    public Task WriteAsync(string path, string content, CancellationToken cancellationToken = default)
        => _fileSystemProvider.WriteAllTextAsync(_workingDirectory.ResolvePath(path), content, cancellationToken);

    /// <inheritdoc />
    public Task<bool> ExistsAsync(string path, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var resolvedPath = _workingDirectory.ResolvePath(path);
        return Task.FromResult(
            _fileSystemProvider.FileExists(resolvedPath)
            || _fileSystemProvider.DirectoryExists(resolvedPath));
    }

    /// <inheritdoc />
    public IZipContext Zip { get; } = zip;

    /// <inheritdoc />
    public IChecksumContext Checksum { get; } = checksum;
}
