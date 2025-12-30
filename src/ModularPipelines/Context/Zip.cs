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
        Directory.CreateDirectory(outputFolderPath);

        ZipFile.ExtractToDirectory(zipPath, outputFolderPath, overwriteFiles);

        return new Folder(outputFolderPath);
    }
}
