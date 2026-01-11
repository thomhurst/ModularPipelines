using System.IO.Compression;
using ModularPipelines.FileSystem;
using ModularPipelines.Helpers;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Context;

internal class Zip : IZip
{
    private readonly IFileSystemProvider _fileSystemProvider;

    public Zip(IFileSystemProvider fileSystemProvider)
    {
        _fileSystemProvider = fileSystemProvider;
    }

    public File ZipFolder(Folder folder, string outputPath, CompressionLevel compressionLevel)
    {
        _fileSystemProvider.CreateDirectory(outputPath.GetDirectory()!);

        if (outputPath.GetPathType() == PathType.Directory)
        {
            outputPath = _fileSystemProvider.Combine(outputPath, Guid.NewGuid().ToString("N") + ".zip");
        }

        ZipFile.CreateFromDirectory(folder.Path, outputPath, compressionLevel, false);

        if (!_fileSystemProvider.FileExists(outputPath))
        {
            throw new InvalidOperationException($"Failed to create zip file at '{outputPath}'.");
        }

        return new File(outputPath, _fileSystemProvider);
    }

    public Folder UnZipToFolder(string zipPath, string outputFolderPath, bool overwriteFiles)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(zipPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputFolderPath);

        if (!_fileSystemProvider.FileExists(zipPath))
        {
            throw new FileNotFoundException($"Zip file not found: '{zipPath}'", zipPath);
        }

        var destinationDir = Path.GetFullPath(outputFolderPath);
        _fileSystemProvider.CreateDirectory(destinationDir);

        try
        {
            using var archive = ZipFile.OpenRead(zipPath);
            foreach (var entry in archive.Entries)
            {
                var destinationPath = Path.GetFullPath(Path.Combine(destinationDir, entry.FullName));

                // Validate against Zip Slip attack (path traversal)
                if (!destinationPath.StartsWith(destinationDir + Path.DirectorySeparatorChar, StringComparison.Ordinal) &&
                    !destinationPath.Equals(destinationDir, StringComparison.Ordinal))
                {
                    throw new InvalidOperationException($"Zip entry '{entry.FullName}' would extract outside the target directory.");
                }

                // Handle directory entries
                if (string.IsNullOrEmpty(entry.Name))
                {
                    _fileSystemProvider.CreateDirectory(destinationPath);
                    continue;
                }

                // Ensure parent directory exists
                var parentDir = Path.GetDirectoryName(destinationPath);
                if (!string.IsNullOrEmpty(parentDir))
                {
                    _fileSystemProvider.CreateDirectory(parentDir);
                }

                // Extract file
                entry.ExtractToFile(destinationPath, overwriteFiles);
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

        return new Folder(outputFolderPath, _fileSystemProvider);
    }
}
