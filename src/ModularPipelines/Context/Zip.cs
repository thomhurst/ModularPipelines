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

        if (!System.IO.File.Exists(zipPath))
        {
            throw new FileNotFoundException($"Zip file not found: '{zipPath}'", zipPath);
        }

        Directory.CreateDirectory(outputFolderPath);

        try
        {
            ZipFile.ExtractToDirectory(zipPath, outputFolderPath, overwriteFiles);
        }
        catch (InvalidDataException ex)
        {
            throw new InvalidDataException($"Failed to extract zip file '{zipPath}': The archive may be corrupt or not a valid zip file.", ex);
        }
        catch (IOException ex) when (!overwriteFiles)
        {
            throw new IOException($"Failed to extract zip file '{zipPath}': A file already exists in the destination. Set overwriteFiles to true to overwrite existing files.", ex);
        }

        return new Folder(outputFolderPath);
    }
}
