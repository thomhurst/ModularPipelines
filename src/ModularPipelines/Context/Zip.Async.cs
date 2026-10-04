using System.IO.Compression;
using ModularPipelines.FileSystem;

namespace ModularPipelines.Context;

internal partial class Zip
{
    public async Task<FilePath> CreateFromDirectoryAsync(
        FolderPath folder,
        string outputPath,
        CompressionLevel compressionLevel = CompressionLevel.Optimal,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        outputPath = PrepareOutputPath(outputPath);
        var directories = _fileSystemProvider.EnumerateDirectories(folder.Path, "*", SearchOption.AllDirectories).ToArray();
        var files = _fileSystemProvider.EnumerateFiles(folder.Path, "*", SearchOption.AllDirectories).ToArray();
        cancellationToken.ThrowIfCancellationRequested();

        var output = _fileSystemProvider.Open(outputPath, FileMode.CreateNew, FileAccess.ReadWrite);
        await using (output.ConfigureAwait(false))
        {
            var archive = await ZipArchive.CreateAsync(output, ZipArchiveMode.Create, leaveOpen: true,
                entryNameEncoding: null, cancellationToken).ConfigureAwait(false);
            await using (archive.ConfigureAwait(false))
            {
                foreach (var directory in directories)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    archive.CreateEntry(NormalizeEntryName(_fileSystemProvider.GetRelativePath(folder.Path, directory)) + "/");
                }

                foreach (var file in files)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var entry = archive.CreateEntry(NormalizeEntryName(_fileSystemProvider.GetRelativePath(folder.Path, file)), compressionLevel);
                    if (_fileSystemProvider is SystemFileSystemProvider)
                    {
                        entry.LastWriteTime = System.IO.File.GetLastWriteTime(file);
                    }

                    var source = _fileSystemProvider.OpenRead(file);
                    await using (source.ConfigureAwait(false))
                    {
                        var destination = await entry.OpenAsync(cancellationToken).ConfigureAwait(false);
                        await using (destination.ConfigureAwait(false))
                        {
                            await source.CopyToAsync(destination, cancellationToken).ConfigureAwait(false);
                        }
                    }
                }
            }
        }

        cancellationToken.ThrowIfCancellationRequested();
        if (!_fileSystemProvider.FileExists(outputPath))
        {
            throw new InvalidOperationException($"Failed to create zip file at '{outputPath}'.");
        }

        return new FilePath(outputPath, _fileSystemProvider);
    }

    public async Task<FolderPath> ExtractToDirectoryAsync(
        string zipPath,
        string outputFolderPath,
        bool overwriteFiles = true,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentException.ThrowIfNullOrWhiteSpace(zipPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputFolderPath);
        zipPath = _workingDirectory.ResolvePath(zipPath);
        outputFolderPath = _workingDirectory.ResolvePath(outputFolderPath);
        if (!_fileSystemProvider.FileExists(zipPath))
        {
            throw new FileNotFoundException($"Zip file not found: '{zipPath}'", zipPath);
        }

        var destinationDirectory = Path.GetFullPath(outputFolderPath);
        _fileSystemProvider.CreateDirectory(destinationDirectory);
        try
        {
            var zipStream = _fileSystemProvider.OpenRead(zipPath);
            await using (zipStream.ConfigureAwait(false))
            {
                var archive = await ZipArchive.CreateAsync(zipStream, ZipArchiveMode.Read, leaveOpen: true,
                    entryNameEncoding: null, cancellationToken).ConfigureAwait(false);
                await using (archive.ConfigureAwait(false))
                {
                    // .NET 10 has no asynchronous entry enumeration API. Entries reads
                    // central-directory metadata synchronously; entry contents use async I/O.
                    foreach (var entry in archive.Entries)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        await ExtractEntryAsync(entry, destinationDirectory, overwriteFiles, cancellationToken).ConfigureAwait(false);
                    }
                }
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

        cancellationToken.ThrowIfCancellationRequested();
        return new FolderPath(outputFolderPath, _fileSystemProvider);
    }

    private async Task ExtractEntryAsync(ZipArchiveEntry entry, string destinationDirectory, bool overwriteFiles, CancellationToken cancellationToken)
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

        var source = await entry.OpenAsync(cancellationToken).ConfigureAwait(false);
        await using (source.ConfigureAwait(false))
        {
            var destination = _fileSystemProvider.Open(destinationPath, overwriteFiles ? FileMode.Create : FileMode.CreateNew, FileAccess.Write);
            await using (destination.ConfigureAwait(false))
            {
                await source.CopyToAsync(destination, cancellationToken).ConfigureAwait(false);
            }
        }

        if (_fileSystemProvider is SystemFileSystemProvider)
        {
            System.IO.File.SetLastWriteTime(destinationPath, entry.LastWriteTime.LocalDateTime);
        }
    }
}
