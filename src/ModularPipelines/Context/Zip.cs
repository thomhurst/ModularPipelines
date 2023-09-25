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

        return new File(outputPath);
    }

    public Folder UnZipToFolder(string zipPath, string outputFolderPath)
    {
        Directory.CreateDirectory(outputFolderPath);

        ZipFile.ExtractToDirectory(zipPath, outputFolderPath, true);

        return new Folder(outputFolderPath);
    }
}
