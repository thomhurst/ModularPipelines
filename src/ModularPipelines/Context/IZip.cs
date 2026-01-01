using System.IO.Compression;
using ModularPipelines.FileSystem;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Context;

/// <summary>
/// Provides ZIP compression and decompression functionality for folders and files.
/// </summary>
public interface IZip
{
    /// <summary>
    /// Compresses a folder into a ZIP file using optimal compression level.
    /// </summary>
    /// <param name="folder">The folder to compress.</param>
    /// <param name="outputPath">The path where the ZIP file will be created.</param>
    /// <returns>A <see cref="File"/> representing the created ZIP file.</returns>
    public File ZipFolder(Folder folder, string outputPath) => ZipFolder(folder, outputPath, CompressionLevel.Optimal);

    /// <summary>
    /// Compresses a folder into a ZIP file with the specified compression level.
    /// </summary>
    /// <param name="folder">The folder to compress.</param>
    /// <param name="outputPath">The path where the ZIP file will be created.</param>
    /// <param name="compressionLevel">The level of compression to use.</param>
    /// <returns>A <see cref="File"/> representing the created ZIP file.</returns>
    public File ZipFolder(Folder folder, string outputPath, CompressionLevel compressionLevel);

    /// <summary>
    /// Extracts a ZIP file to a folder, overwriting existing files by default.
    /// </summary>
    /// <param name="zipPath">The path to the ZIP file to extract.</param>
    /// <param name="outputFolderPath">The path where the contents will be extracted.</param>
    /// <returns>A <see cref="Folder"/> representing the extraction destination folder.</returns>
    public Folder UnZipToFolder(string zipPath, string outputFolderPath) => UnZipToFolder(zipPath, outputFolderPath, true);

    /// <summary>
    /// Extracts a ZIP file to a folder with control over whether to overwrite existing files.
    /// </summary>
    /// <param name="zipPath">The path to the ZIP file to extract.</param>
    /// <param name="outputFolderPath">The path where the contents will be extracted.</param>
    /// <param name="overwriteFiles">If <c>true</c>, existing files will be overwritten; otherwise, an exception is thrown for conflicts.</param>
    /// <returns>A <see cref="Folder"/> representing the extraction destination folder.</returns>
    public Folder UnZipToFolder(string zipPath, string outputFolderPath, bool overwriteFiles);
}