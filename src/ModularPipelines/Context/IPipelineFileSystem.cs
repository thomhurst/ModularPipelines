using ModularPipelines.Helpers;

namespace ModularPipelines.Context;

/// <summary>
/// Provides access to file system operations.
/// </summary>
/// <remarks>
/// This interface groups file system, zip, and checksum helpers
/// for better Interface Segregation.
/// </remarks>
public interface IPipelineFileSystem
{
    /// <summary>
    /// Gets helpers for interacting with the filesystem.
    /// </summary>
    /// <remarks>
    /// Provides file/directory operations with logging and error handling.
    /// </remarks>
    IFileSystemContext FileSystem { get; }

    /// <summary>
    /// Gets helpers for zipping and unzipping files and directories.
    /// </summary>
    IZip Zip { get; }

    /// <summary>
    /// Gets helpers for checking the Checksum of a file.
    /// </summary>
    IChecksum Checksum { get; }
}
