using System.IO.Compression;
using ModularPipelines.FileSystem;

namespace ModularPipelines.Context;

public interface IZip
{
    public void ZipFolder(Folder folder, string outputPath) => ZipFolder(folder, outputPath, CompressionLevel.Optimal);
    public void ZipFolder(Folder folder, string outputPath, CompressionLevel compressionLevel);

    public void UnZipToFolder(string zipPath, string outputFolderPath);
}
