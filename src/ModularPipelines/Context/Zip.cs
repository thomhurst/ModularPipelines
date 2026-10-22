using System.IO.Compression;
using System.Security.AccessControl;
using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using ModularPipelines.Helpers;

namespace ModularPipelines.Context;

internal partial class Zip(
    IFileSystemProvider fileSystemProvider,
    PipelineWorkingDirectory workingDirectory,
    Action<string, string, string?>? replaceFile = null) : IZipContext
{
    private const int UnixFileTypeRegular = 0x8000;
    private const int UnixPermissionMask = 0x1FF;
    private const int UnixHostPlatform = 3;
    private readonly IFileSystemProvider _fileSystemProvider = fileSystemProvider;
    private readonly PipelineWorkingDirectory _workingDirectory = workingDirectory;
    private readonly Action<string, string, string?> _replaceFile = replaceFile ?? System.IO.File.Replace;

    public FilePath CreateFromDirectory(FolderPath folder, string outputPath, CompressionLevel compressionLevel)
    {
        var (directories, files) = SnapshotSourceEntries(folder.Path);
        outputPath = PrepareOutputPath(outputPath);

        var temporaryPath = GetTemporaryArchivePath(outputPath);
        var output = _fileSystemProvider.Open(temporaryPath, FileMode.CreateNew, FileAccess.ReadWrite);
        try
        {
            using (output)
            using (var archive = new ZipArchive(output, ZipArchiveMode.Create))
            {
                foreach (var directory in directories)
                {
                    var entryName = NormalizeEntryName(
                        _fileSystemProvider.GetRelativePath(folder.Path, directory)) + "/";
                    archive.CreateEntry(entryName);
                }

                foreach (var file in files)
                {
                    var entryName = NormalizeEntryName(
                        _fileSystemProvider.GetRelativePath(folder.Path, file));
                    var entry = archive.CreateEntry(entryName, compressionLevel);
                    StoreFileMetadata(entry, file);

                    using var source = _fileSystemProvider.OpenRead(file);
                    using var destination = entry.Open();
                    source.CopyTo(destination);
                }
            }

            _fileSystemProvider.MoveFile(temporaryPath, outputPath);
        }
        finally
        {
            _fileSystemProvider.DeleteFile(temporaryPath);
        }

        if (!_fileSystemProvider.FileExists(outputPath))
        {
            throw new InvalidOperationException($"Failed to create zip file at '{outputPath}'.");
        }

        return new FilePath(outputPath, _fileSystemProvider);
    }

    public FolderPath ExtractToDirectory(string zipPath, string outputFolderPath, bool overwriteFiles)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(zipPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputFolderPath);
        zipPath = _workingDirectory.ResolvePath(zipPath);
        outputFolderPath = _workingDirectory.ResolvePath(outputFolderPath);

        if (!_fileSystemProvider.FileExists(zipPath))
        {
            throw new FileNotFoundException($"Zip file not found: '{zipPath}'", zipPath);
        }

        var destinationDir = Path.GetFullPath(outputFolderPath);
        _fileSystemProvider.CreateDirectory(destinationDir);

        try
        {
            using var zipStream = _fileSystemProvider.OpenRead(zipPath);
            using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);
            foreach (var entry in archive.Entries)
            {
                ExtractEntry(entry, destinationDir, overwriteFiles);
            }
        }
        catch (InvalidDataException ex)
        {
            throw new InvalidDataException($"Failed to extract zip file '{zipPath}': The archive may be corrupt or not a valid zip file.", ex);
        }
        catch (DestinationFileExistsException ex)
        {
            throw new IOException($"Failed to extract zip file '{zipPath}': A file already exists in the destination. Set overwriteFiles to true to overwrite existing files.", ex);
        }
        catch (IOException ex)
        {
            throw new IOException($"Failed to extract zip file '{zipPath}': An I/O error occurred while extracting the archive.", ex);
        }

        return new FolderPath(outputFolderPath, _fileSystemProvider);
    }

    private void ExtractEntry(
        ZipArchiveEntry entry,
        string destinationDirectory,
        bool overwriteFiles)
    {
        var destinationPath = GetValidatedDestinationPath(entry, destinationDirectory);
        if (string.IsNullOrEmpty(entry.Name))
        {
            _fileSystemProvider.CreateDirectory(destinationPath);
            return;
        }

        var parentDirectory = Path.GetDirectoryName(destinationPath);
        if (!string.IsNullOrEmpty(parentDirectory))
        {
            _fileSystemProvider.CreateDirectory(parentDirectory);
        }

        var extractionPath = GetExtractionPath(destinationPath);
        using var source = entry.Open();
        var destination = OpenExtractionDestination(entry, destinationPath, extractionPath, overwriteFiles);
        try
        {
            using (destination)
            {
                source.CopyTo(destination);
            }

            CompleteExtraction(entry, extractionPath, destinationPath, overwriteFiles);
        }
        finally
        {
            _fileSystemProvider.DeleteFile(extractionPath);
        }
    }

    private void StoreFileMetadata(ZipArchiveEntry entry, string path)
    {
        if (_fileSystemProvider is SystemFileSystemProvider)
        {
            entry.LastWriteTime = System.IO.File.GetLastWriteTime(path);
            if (!OperatingSystem.IsWindows())
            {
                entry.ExternalAttributes = (UnixFileTypeRegular | (int) System.IO.File.GetUnixFileMode(path)) << 16;
            }
        }
    }

    private void RestoreFileMetadata(ZipArchiveEntry entry, string destinationPath)
    {
        if (_fileSystemProvider is SystemFileSystemProvider)
        {
            System.IO.File.SetLastWriteTime(
                destinationPath,
                entry.LastWriteTime.LocalDateTime);
        }
    }

    private static string GetExtractionPath(string destinationPath) =>
        Path.Combine(Path.GetDirectoryName(destinationPath)!, $".modularpipelines-extract-{Guid.NewGuid():N}.tmp");

    private Stream OpenExtractionDestination(
        ZipArchiveEntry entry, string destinationPath, string extractionPath, bool overwriteFiles)
    {
        if (!overwriteFiles && _fileSystemProvider.FileExists(destinationPath))
        {
            throw new DestinationFileExistsException(destinationPath);
        }

        try
        {
            if (_fileSystemProvider is SystemFileSystemProvider)
            {
                if (OperatingSystem.IsWindows() && overwriteFiles && System.IO.File.Exists(destinationPath))
                {
                    var security = new FileInfo(destinationPath).GetAccessControl(AccessControlSections.Access);
                    // Protect partially extracted contents with the destination's permissions from creation.
                    security.SetAccessRuleProtection(isProtected: true, preserveInheritance: true);
                    return new FileInfo(extractionPath).Create(
                        FileMode.CreateNew, FileSystemRights.Write, FileShare.None, 4096, FileOptions.None, security);
                }

                var options = new FileStreamOptions
                {
                    Access = FileAccess.Write,
                    Mode = FileMode.CreateNew,
                    Share = FileShare.None,
                };
                var unixAttributes = (entry.ExternalAttributes >> 16) & 0xFFFF;
                if (!OperatingSystem.IsWindows() && (entry.VersionMadeBy >> 8) == UnixHostPlatform && unixAttributes != 0)
                {
                    // Only ordinary permissions are trusted; creation lets the OS apply umask.
                    var permissions = unixAttributes & UnixPermissionMask;
                    options.UnixCreateMode = (UnixFileMode) permissions;
                }

                return new FileStream(extractionPath, options);
            }

            return _fileSystemProvider.Open(extractionPath, FileMode.CreateNew, FileAccess.Write);
        }
        catch (IOException ex) when (!overwriteFiles && _fileSystemProvider.FileExists(destinationPath))
        {
            // A competing writer may create the file after the existence check.
            throw new DestinationFileExistsException(destinationPath, ex);
        }
    }

    private void CompleteExtraction(ZipArchiveEntry entry, string extractionPath, string destinationPath, bool overwriteFiles)
    {
        RestoreFileMetadata(entry, extractionPath);
        try
        {
            if (_fileSystemProvider is SystemFileSystemProvider
                && OperatingSystem.IsWindows() && overwriteFiles && System.IO.File.Exists(destinationPath))
            {
                // ReplaceFile preserves the destination's ACL and inheritance policy.
                ReplaceExtraction(extractionPath, destinationPath);
            }
            else
            {
                // Publish only the completed file through its provider, preserving atomic overwrite semantics.
                _fileSystemProvider.MoveFile(extractionPath, destinationPath, overwriteFiles);
            }
        }
        catch (IOException ex) when (!overwriteFiles && _fileSystemProvider.FileExists(destinationPath))
        {
            throw new DestinationFileExistsException(destinationPath, ex);
        }
    }

    private void ReplaceExtraction(string extractionPath, string destinationPath)
    {
        var backupPath = GetExtractionPath(destinationPath);
        try
        {
            _replaceFile(extractionPath, destinationPath, backupPath);
        }
        catch (IOException publicationException)
        {
            // ReplaceFile can move the original to the backup before failing to publish the replacement.
            // Never overwrite a competing writer while restoring it, or delete the remaining backup.
            if (System.IO.File.Exists(backupPath))
            {
                try
                {
                    System.IO.File.Move(backupPath, destinationPath);
                }
                catch (Exception recoveryException) when (recoveryException is IOException or UnauthorizedAccessException)
                {
                    throw new IOException(
                        $"Failed to publish '{destinationPath}'. The original file is retained at '{backupPath}'.",
                        new AggregateException(publicationException, recoveryException));
                }
            }

            throw;
        }

        System.IO.File.Delete(backupPath);
    }

    private sealed class DestinationFileExistsException(string path, Exception? innerException = null)
        : IOException($"The file '{path}' already exists.", innerException);

    private static string GetTemporaryArchivePath(string outputPath) =>
        Path.Combine(Path.GetDirectoryName(outputPath)!, $"{Guid.NewGuid():N}.tmp");

    private string GetValidatedDestinationPath(
        ZipArchiveEntry entry,
        string destinationDirectory)
    {
        var destinationPath = Path.GetFullPath(
            Path.Combine(destinationDirectory, entry.FullName));
        if (Path.TrimEndingDirectorySeparator(destinationPath).Equals(
                Path.TrimEndingDirectorySeparator(destinationDirectory), StringComparison.Ordinal))
        {
            // Return the trusted root rather than the entry-derived spelling.
            return destinationDirectory;
        }

        var destinationPrefix = Path.EndsInDirectorySeparator(destinationDirectory)
            ? destinationDirectory
            : destinationDirectory + Path.DirectorySeparatorChar;
        if (!destinationPath.StartsWith(
                destinationPrefix,
                StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                $"Zip entry '{entry.FullName}' would extract outside the target directory.");
        }

        // Walk from the trusted root so no metadata query follows an unchecked ancestor.
        // Inspect attributes directly: existence checks can hide dangling links.
        var current = Path.TrimEndingDirectorySeparator(destinationDirectory);
        var relativePath = Path.GetRelativePath(destinationDirectory, destinationPath);
        foreach (var segment in relativePath.Split([Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar], StringSplitOptions.RemoveEmptyEntries))
        {
            current = Path.Combine(current, segment);
            try
            {
                var attributes = _fileSystemProvider.GetAttributes(current);
                // FileSystemInfo.Attributes returns -1 for missing entries, with every bit set.
                if ((int) attributes != -1 && (attributes & FileAttributes.ReparsePoint) != 0)
                {
                    throw new InvalidOperationException(
                        $"Zip entry '{entry.FullName}' would extract through a symbolic link or reparse point at '{current}'.");
                }
            }
            catch (FileNotFoundException)
            {
                // Missing components will be created after all existing components are validated.
            }
            catch (DirectoryNotFoundException)
            {
                // A parent component does not exist yet.
            }
        }

        return destinationPath;
    }

    private (List<string> Directories, List<string> Files) SnapshotSourceEntries(
        string sourcePath, CancellationToken cancellationToken = default)
    {
        // Validate the entire source before preparing output. Enumerate one level at a time
        // so directory links (including cycles) are rejected before descent.
        cancellationToken.ThrowIfCancellationRequested();
        ValidateSourceEntry(sourcePath);
        var directories = new List<string>();
        var files = new List<string>();
        var pending = new Stack<string>();
        pending.Push(sourcePath);
        while (pending.TryPop(out var current))
        {
            cancellationToken.ThrowIfCancellationRequested();
            foreach (var directory in _fileSystemProvider.EnumerateDirectories(current, "*", SearchOption.TopDirectoryOnly))
            {
                cancellationToken.ThrowIfCancellationRequested();
                ValidateSourceEntry(directory);
                directories.Add(directory);
                pending.Push(directory);
            }

            foreach (var file in _fileSystemProvider.EnumerateFiles(current, "*", SearchOption.TopDirectoryOnly))
            {
                cancellationToken.ThrowIfCancellationRequested();
                ValidateSourceEntry(file);
                files.Add(file);
            }
        }

        cancellationToken.ThrowIfCancellationRequested();
        return (directories, files);
    }

    private void ValidateSourceEntry(string path)
    {
        if ((_fileSystemProvider.GetAttributes(path) & FileAttributes.ReparsePoint) != 0)
        {
            throw new IOException($"Cannot create a ZIP archive containing a source link: '{path}'.");
        }
    }

    private string PrepareOutputPath(string outputPath)
    {
        outputPath = _workingDirectory.ResolvePath(outputPath);
        var outputIsDirectory = _fileSystemProvider.DirectoryExists(outputPath)
                                || (!_fileSystemProvider.FileExists(outputPath)
                                    && IsDirectoryPath(outputPath));
        if (outputIsDirectory)
        {
            outputPath = _fileSystemProvider.Combine(outputPath, Guid.NewGuid().ToString("N") + ".zip");
        }

        if (_fileSystemProvider.FileExists(outputPath))
        {
            throw new IOException($"The file '{outputPath}' already exists.");
        }

        _fileSystemProvider.CreateDirectory(outputPath.GetDirectory()!);
        return outputPath;
    }

    private static string NormalizeEntryName(string path) =>
        path.Replace(Path.DirectorySeparatorChar, '/');

    private static bool IsDirectoryPath(string path) =>
        PathHelpers.EndsWithDirectorySeparator(path) || !Path.HasExtension(path);
}
