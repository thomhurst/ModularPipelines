using System.Security.Cryptography;
using ModularPipelines.Distributed.Models;
using ModularPipelines.FileSystem;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Distributed.Helpers;

/// <summary>
/// Helper methods for transferring files between orchestrator and worker nodes.
/// </summary>
internal static class FileTransferHelper
{
    /// <summary>
    /// Prepares files for transfer by reading their content and computing hashes.
    /// </summary>
    /// <param name="files">The files to prepare for transfer.</param>
    /// <param name="baseDirectory">The base directory to calculate relative paths from.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A dictionary of file transfer information, keyed by relative path.</returns>
    public static async Task<Dictionary<string, FileTransferInfo>> PrepareFilesForTransferAsync(
        IEnumerable<File> files,
        string baseDirectory,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(files);
        ArgumentException.ThrowIfNullOrWhiteSpace(baseDirectory);

        var result = new Dictionary<string, FileTransferInfo>();
        var filesList = files.ToList();

        foreach (var file in filesList)
        {
            if (!System.IO.File.Exists(file.Path))
            {
                continue;
            }

            var content = await file.ReadBytesAsync(cancellationToken);
            var relativePath = Path.GetRelativePath(baseDirectory, file.Path);

            // Compute SHA256 hash for integrity verification
            var hash = Convert.ToBase64String(SHA256.HashData(content));

            result[relativePath] = new FileTransferInfo
            {
                RelativePath = relativePath,
                Content = content,
                ContentHash = hash,
            };
        }

        return result;
    }

    /// <summary>
    /// Writes transferred files to the specified target directory.
    /// </summary>
    /// <param name="transferredFiles">The files to write.</param>
    /// <param name="targetDirectory">The target directory to write files to.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of the written file objects.</returns>
    public static async Task<List<File>> WriteTransferredFilesAsync(
        Dictionary<string, FileTransferInfo> transferredFiles,
        string targetDirectory,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(transferredFiles);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetDirectory);

        var writtenFiles = new List<File>();

        foreach (var (relativePath, fileInfo) in transferredFiles)
        {
            var targetPath = Path.Combine(targetDirectory, relativePath);

            // Ensure directory exists
            var targetDir = Path.GetDirectoryName(targetPath);
            if (!string.IsNullOrEmpty(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            // Verify hash if provided
            if (!string.IsNullOrEmpty(fileInfo.ContentHash))
            {
                var computedHash = Convert.ToBase64String(SHA256.HashData(fileInfo.Content));
                if (computedHash != fileInfo.ContentHash)
                {
                    throw new InvalidOperationException(
                        $"File integrity check failed for {relativePath}. Expected hash: {fileInfo.ContentHash}, Computed: {computedHash}");
                }
            }

            // Write file
            var file = new File(targetPath);
            await file.WriteAsync(fileInfo.Content, cancellationToken);

            writtenFiles.Add(file);
        }

        return writtenFiles;
    }
}
