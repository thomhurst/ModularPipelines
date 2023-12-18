using System.IO.Compression;
using ModularPipelines.FileSystem;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Context;

public interface IZip
{
    public File ZipFolder(Folder folder, string outputPath) => ZipFolder(folder, outputPath, CompressionLevel.Optimal);

    public File ZipFolder(Folder folder, string outputPath, CompressionLevel compressionLevel);

    public Folder UnZipToFolder(string zipPath, string outputFolderPath) => UnZipToFolder(zipPath, outputFolderPath, true);

    public Folder UnZipToFolder(string zipPath, string outputFolderPath, bool overwriteFiles);
}