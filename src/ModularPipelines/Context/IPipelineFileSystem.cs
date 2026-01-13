using ModularPipelines.Helpers;

namespace ModularPipelines.Context;

/// <summary>
/// Provides access to file system operations.
/// </summary>
/// <remarks>
/// <para>
/// This interface groups file system, zip, and checksum helpers
/// for better Interface Segregation.
/// </para>
/// <para><b>Thread Safety:</b></para>
/// <para>
/// All members of this interface provide thread-safe access:
/// </para>
/// <list type="bullet">
/// <item><description><see cref="FileSystem"/>: Read operations are fully thread-safe. Write operations are thread-safe at the API level, but when multiple threads write to the same file, coordinate access to avoid data races.</description></item>
/// <item><description><see cref="Zip"/>: Thread-safe for concurrent zip/unzip operations on different files.</description></item>
/// <item><description><see cref="Checksum"/>: Thread-safe for all checksum calculations.</description></item>
/// </list>
/// <para>
/// <b>Best Practice:</b> When performing parallel file operations, ensure each thread works on different files,
/// or use appropriate synchronization when multiple threads must access the same file.
/// </para>
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
