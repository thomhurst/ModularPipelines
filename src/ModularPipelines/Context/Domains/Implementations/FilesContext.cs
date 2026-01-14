using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using ModularPipelines.Context.Domains.Files;
using ModularPipelines.FileSystem;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Context.Domains.Implementations;

/// <summary>
/// Provides file system operations with rich File and Folder return types.
/// </summary>
internal class FilesContext : IFilesContext
{
    private readonly IFileSystemContext _fileSystemContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="FilesContext"/> class.
    /// </summary>
    /// <param name="fileSystemContext">The file system context for basic file operations.</param>
    /// <param name="zip">The zip context for compression operations.</param>
    /// <param name="checksum">The checksum context for file checksum operations.</param>
    public FilesContext(
        IFileSystemContext fileSystemContext,
        IZipContext zip,
        IChecksumContext checksum)
    {
        _fileSystemContext = fileSystemContext;
        Zip = zip;
        Checksum = checksum;
    }

    /// <inheritdoc />
    public File GetFile(string path) => _fileSystemContext.GetFile(path);

    /// <inheritdoc />
    public Folder GetFolder(string path) => _fileSystemContext.GetFolder(path);

    /// <inheritdoc />
    public Folder GetFolder(System.Environment.SpecialFolder specialFolder) => _fileSystemContext.GetFolder(specialFolder);

    /// <inheritdoc />
    public IEnumerable<File> Glob(string pattern)
    {
        // Use the current directory as the root for globbing
        var currentDirectory = Directory.GetCurrentDirectory();
        var directoryInfo = new DirectoryInfo(currentDirectory);

        return new Matcher(StringComparison.OrdinalIgnoreCase)
            .AddInclude(pattern)
            .Execute(new DirectoryInfoWrapper(directoryInfo))
            .Files
            .Select(x => new File(Path.Combine(currentDirectory, x.Path)))
            .Distinct();
    }

    /// <inheritdoc />
    public IEnumerable<Folder> GlobFolders(string pattern)
    {
        // Use the current directory as the root for globbing
        var currentDirectory = Directory.GetCurrentDirectory();
        var directoryInfo = new DirectoryInfo(currentDirectory);

        // For folder globbing, we need to handle patterns that match directories
        // The Matcher is designed for files, so we match files and then extract unique parent directories
        // Alternatively, we enumerate directories and filter by pattern
        return EnumerateFoldersMatchingPattern(directoryInfo, pattern)
            .Select(x => new Folder(x.FullName))
            .Distinct();
    }

    /// <inheritdoc />
    public Task<string> ReadAsync(string path, CancellationToken cancellationToken = default)
        => System.IO.File.ReadAllTextAsync(path, cancellationToken);

    /// <inheritdoc />
    public Task WriteAsync(string path, string content, CancellationToken cancellationToken = default)
        => System.IO.File.WriteAllTextAsync(path, content, cancellationToken);

    /// <inheritdoc />
    public Task<bool> ExistsAsync(string path, CancellationToken cancellationToken = default)
        => Task.FromResult(System.IO.File.Exists(path) || Directory.Exists(path));

    /// <inheritdoc />
    public IZipContext Zip { get; }

    /// <inheritdoc />
    public IChecksumContext Checksum { get; }

    /// <summary>
    /// Enumerates directories matching a glob pattern.
    /// </summary>
    /// <param name="rootDirectory">The root directory to search from.</param>
    /// <param name="pattern">The glob pattern to match.</param>
    /// <returns>An enumerable of matching directories.</returns>
    private static IEnumerable<DirectoryInfo> EnumerateFoldersMatchingPattern(DirectoryInfo rootDirectory, string pattern)
    {
        // Use Matcher to find files matching the pattern with a wildcard appended
        // This will match any file in directories that match the pattern
        var matcher = new Matcher(StringComparison.OrdinalIgnoreCase)
            .AddInclude(pattern.TrimEnd('/') + "/**/*");

        var result = matcher.Execute(new DirectoryInfoWrapper(rootDirectory));

        // Extract unique parent directories from matched files
        var matchedDirs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var file in result.Files)
        {
            // Get the directory part of the matched file relative to root
            var relativePath = file.Path;
            var directoryPath = Path.GetDirectoryName(relativePath);

            if (!string.IsNullOrEmpty(directoryPath))
            {
                // Walk up the directory path and check if it matches the original pattern
                var fullPath = Path.Combine(rootDirectory.FullName, directoryPath);
                if (Directory.Exists(fullPath))
                {
                    matchedDirs.Add(fullPath);
                }
            }
        }

        // Also try direct directory enumeration for patterns that might not have files
        try
        {
            // Convert glob pattern to search pattern (simplified)
            var searchPattern = pattern.Replace("**", "*").TrimEnd('/');
            if (searchPattern.Contains('/'))
            {
                searchPattern = Path.GetFileName(searchPattern);
            }

            foreach (var dir in rootDirectory.EnumerateDirectories(searchPattern, SearchOption.AllDirectories))
            {
                matchedDirs.Add(dir.FullName);
            }
        }
        catch (ArgumentException)
        {
            // Invalid search pattern characters - ignore and use only matcher results
        }

        return matchedDirs.Select(x => new DirectoryInfo(x));
    }
}
