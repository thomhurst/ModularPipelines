using System.IO.Compression;
using ModularPipelines.FileSystem;

namespace ModularPipelines.Context;

/// <summary>
/// Provides ZIP compression and decompression functionality for folders and files.
/// </summary>
public interface IZipContext
{
    /// <summary>
    /// Compresses a folder into a ZIP file using optimal compression level.
    /// </summary>
    /// <param name="folder">The folder to compress.</param>
    /// <param name="outputPath">The path where the ZIP file will be created.</param>
    /// <returns>A <see cref="FilePath"/> representing the created ZIP file.</returns>
    FilePath CreateFromDirectory(FolderPath folder, string outputPath) =>
        CreateFromDirectory(folder, outputPath, CompressionLevel.Optimal);

    /// <summary>
    /// Compresses a folder into a ZIP file with the specified compression level.
    /// </summary>
    /// <param name="folder">The folder to compress.</param>
    /// <param name="outputPath">The path where the ZIP file will be created.</param>
    /// <param name="compressionLevel">The level of compression to use.</param>
    /// <returns>A <see cref="FilePath"/> representing the created ZIP file.</returns>
    FilePath CreateFromDirectory(FolderPath folder, string outputPath, CompressionLevel compressionLevel);

    /// <summary>
    /// Extracts a ZIP file to a folder, overwriting existing files by default.
    /// </summary>
    /// <param name="zipPath">The path to the ZIP file to extract.</param>
    /// <param name="outputFolderPath">The path where the contents will be extracted.</param>
    /// <returns>A <see cref="FolderPath"/> representing the extraction destination folder.</returns>
    FolderPath ExtractToDirectory(string zipPath, string outputFolderPath) =>
        ExtractToDirectory(zipPath, outputFolderPath, true);

    /// <summary>
    /// Extracts a ZIP file to a folder with control over whether to overwrite existing files.
    /// </summary>
    /// <param name="zipPath">The path to the ZIP file to extract.</param>
    /// <param name="outputFolderPath">The path where the contents will be extracted.</param>
    /// <param name="overwriteFiles">If <c>true</c>, existing files will be overwritten; otherwise, an exception is thrown for conflicts.</param>
    /// <returns>A <see cref="FolderPath"/> representing the extraction destination folder.</returns>
    FolderPath ExtractToDirectory(string zipPath, string outputFolderPath, bool overwriteFiles);

    /// <summary>Asynchronously compresses a folder into a ZIP file.</summary>
    /// <param name="folder">The resolved source folder.</param>
    /// <param name="outputPath">The output path, relative to the pipeline working directory when not absolute.</param>
    /// <param name="compressionLevel">The compression level for file entries.</param>
    /// <param name="cancellationToken">A token used to cancel compression and stream copying.</param>
    /// <returns>The created ZIP file.</returns>
    /// <remarks>
    /// Cancellation or failure can leave a partial archive at the destination.
    /// File metadata and directory enumeration are synchronous. The .NET runtime may also
    /// perform synchronous I/O during entry creation and finalization. Cleanup still runs after cancellation.
    /// </remarks>
    Task<FilePath> CreateFromDirectoryAsync(
        FolderPath folder,
        string outputPath,
        CompressionLevel compressionLevel = CompressionLevel.Optimal,
        CancellationToken cancellationToken = default);

    /// <summary>Asynchronously extracts a ZIP file to a folder.</summary>
    /// <param name="zipPath">The archive path, relative to the pipeline working directory when not absolute.</param>
    /// <param name="outputFolderPath">The destination path, relative to the pipeline working directory when not absolute.</param>
    /// <param name="overwriteFiles">Whether to overwrite existing destination files.</param>
    /// <param name="cancellationToken">A token used to cancel extraction and stream copying.</param>
    /// <returns>The extraction destination folder.</returns>
    /// <remarks>
    /// Cancellation or failure can leave files already extracted, including a partial current file.
    /// File metadata and ZIP central-directory enumeration are synchronous; entry contents are copied asynchronously.
    /// </remarks>
    Task<FolderPath> ExtractToDirectoryAsync(
        string zipPath,
        string outputFolderPath,
        bool overwriteFiles = true,
        CancellationToken cancellationToken = default);
}
