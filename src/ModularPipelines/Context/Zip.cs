using System.IO.Compression;
using ModularPipelines.FileSystem;
using ModularPipelines.Helpers;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Context;

internal class Zip : IZip
{
    public File ZipFolder(Folder folder, string outputPath, CompressionLevel compressionLevel)
    {
        Directory.CreateDirectory(outputPath.GetDirectory()!);

        if (outputPath.GetPathType() == PathType.Directory)
        {
            outputPath = Path.Combine(outputPath, Guid.NewGuid().ToString("N") + ".zip");
        }

        ZipFile.CreateFromDirectory(folder.Path, outputPath, compressionLevel, false);

        if (!System.IO.File.Exists(outputPath))
        {
            throw new InvalidOperationException($"Failed to create zip file at '{outputPath}'.");
        }

        return new File(outputPath);
    }

    public Folder UnZipToFolder(string zipPath, string outputFolderPath, bool overwriteFiles)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(zipPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputFolderPath);

        if (!System.IO.File.Exists(zipPath))
        {
            throw new FileNotFoundException($"Zip file not found: '{zipPath}'", zipPath);
        }

        var destinationDir = Path.GetFullPath(outputFolderPath);
        Directory.CreateDirectory(destinationDir);

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
                    Directory.CreateDirectory(destinationPath);
                    continue;
                }

                // Ensure parent directory exists
                var parentDir = Path.GetDirectoryName(destinationPath);
                if (!string.IsNullOrEmpty(parentDir))
                {
                    Directory.CreateDirectory(parentDir);
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

        return new Folder(outputFolderPath);
    }
}
