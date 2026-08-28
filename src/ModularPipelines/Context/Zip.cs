using System.IO.Compression;
using ModularPipelines.Context.Domains.Files;
using ModularPipelines.FileSystem;
using ModularPipelines.Helpers;

namespace ModularPipelines.Context;

internal class Zip(
    IFileSystemProvider fileSystemProvider,
    PipelineWorkingDirectory workingDirectory) : IZipContext
{
    private readonly IFileSystemProvider _fileSystemProvider = fileSystemProvider;
    private readonly PipelineWorkingDirectory _workingDirectory = workingDirectory;

    public FilePath CreateFromDirectory(FolderPath folder, string outputPath, CompressionLevel compressionLevel)
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
        var directories = _fileSystemProvider
            .EnumerateDirectories(folder.Path, "*", SearchOption.AllDirectories)
            .ToArray();
        var files = _fileSystemProvider
            .EnumerateFiles(folder.Path, "*", SearchOption.AllDirectories)
            .ToArray();

        using (var output = _fileSystemProvider.Open(
                   outputPath,
                   FileMode.CreateNew,
                   FileAccess.ReadWrite))
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
                if (_fileSystemProvider is SystemFileSystemProvider)
                {
                    entry.LastWriteTime = System.IO.File.GetLastWriteTime(file);
                }

                using var source = _fileSystemProvider.OpenRead(file);
                using var destination = entry.Open();
                source.CopyTo(destination);
            }
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
        catch (IOException ex) when (!overwriteFiles && ex.Message.Contains("already exists", StringComparison.OrdinalIgnoreCase))
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

        if (_fileSystemProvider.FileExists(destinationPath) && !overwriteFiles)
        {
            throw new IOException($"The file '{destinationPath}' already exists.");
        }

        using (var source = entry.Open())
        using (var destination = _fileSystemProvider.Open(
                   destinationPath,
                   overwriteFiles ? FileMode.Create : FileMode.CreateNew,
                   FileAccess.Write))
        {
            source.CopyTo(destination);
        }

        if (_fileSystemProvider is SystemFileSystemProvider)
        {
            System.IO.File.SetLastWriteTime(
                destinationPath,
                entry.LastWriteTime.LocalDateTime);
        }
    }

    private static string GetValidatedDestinationPath(
        ZipArchiveEntry entry,
        string destinationDirectory)
    {
        var destinationPath = Path.GetFullPath(
            Path.Combine(destinationDirectory, entry.FullName));
        if (!destinationPath.StartsWith(
                destinationDirectory + Path.DirectorySeparatorChar,
                StringComparison.Ordinal)
            && !destinationPath.Equals(destinationDirectory, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                $"Zip entry '{entry.FullName}' would extract outside the target directory.");
        }

        return destinationPath;
    }

    private static string NormalizeEntryName(string path) =>
        path.Replace(Path.DirectorySeparatorChar, '/');

    private static bool IsDirectoryPath(string path) =>
        PathHelpers.EndsWithDirectorySeparator(path) || !Path.HasExtension(path);
}
