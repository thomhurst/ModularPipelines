using System.Runtime.CompilerServices;
using System.Text;

namespace ModularPipelines.FileSystem;

/// <summary>
/// Provides the file system operations used by <see cref="FilePath"/>, <see cref="FolderPath"/>,
/// and the pipeline context. Register a custom implementation, such as an in-memory file system,
/// to redirect those operations in tests.
/// </summary>
/// <remarks>
/// <para>
/// Implementations must provide the primitive operations: opening streams, file and directory
/// management, existence checks, enumeration, and file metadata. The remaining members have default
/// implementations built on those primitives, and <see cref="System.IO.Path"/> for path helpers;
/// override them when the backing store offers a faster or more faithful equivalent.
/// </para>
/// <para>
/// Metadata members apply to both files and directories. Timestamps are expressed in UTC.
/// </para>
/// </remarks>
public interface IFileSystemProvider
{
    /// <summary>
    /// Reads a file as text, detecting the encoding from a byte order mark and defaulting to UTF-8.
    /// </summary>
    /// <param name="path">The file path.</param>
    /// <param name="cancellationToken">A token used to cancel the read.</param>
    /// <returns>The file contents.</returns>
    /// <remarks>The default implementation reads the stream returned by <see cref="OpenRead"/>.</remarks>
    async Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default)
    {
        var stream = OpenRead(path);
        await using (stream.ConfigureAwait(false))
        {
            using var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
            return await reader.ReadToEndAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Reads a file line by line, detecting the encoding from a byte order mark and defaulting to UTF-8.
    /// </summary>
    /// <param name="path">The file path.</param>
    /// <param name="cancellationToken">A token used to cancel the read.</param>
    /// <returns>The file's lines.</returns>
    /// <remarks>The default implementation reads the stream returned by <see cref="OpenRead"/>.</remarks>
    async IAsyncEnumerable<string> ReadLinesAsync(
        string path,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var stream = OpenRead(path);
        await using (stream.ConfigureAwait(false))
        {
            using var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
            while (await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false) is { } line)
            {
                yield return line;
            }
        }
    }

    /// <summary>
    /// Reads a file's bytes.
    /// </summary>
    /// <param name="path">The file path.</param>
    /// <param name="cancellationToken">A token used to cancel the read.</param>
    /// <returns>The file contents.</returns>
    /// <remarks>The default implementation copies the stream returned by <see cref="OpenRead"/>.</remarks>
    async Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken = default)
    {
        var stream = OpenRead(path);
        await using (stream.ConfigureAwait(false))
        {
            using var buffer = new MemoryStream();
            await stream.CopyToAsync(buffer, cancellationToken).ConfigureAwait(false);
            return buffer.ToArray();
        }
    }

    /// <summary>
    /// Creates or overwrites a file with UTF-8 text without a byte order mark.
    /// </summary>
    /// <param name="path">The file path.</param>
    /// <param name="contents">The text to write.</param>
    /// <param name="cancellationToken">A token used to cancel the write.</param>
    /// <returns>A task that completes when the file is written.</returns>
    /// <remarks>The default implementation writes to the stream returned by <see cref="Create"/>.</remarks>
    Task WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken = default) =>
        WriteTextAsync(Create(path), [contents], appendNewLines: false, cancellationToken);

    /// <summary>
    /// Creates or overwrites a file with the specified bytes.
    /// </summary>
    /// <param name="path">The file path.</param>
    /// <param name="contents">The bytes to write.</param>
    /// <param name="cancellationToken">A token used to cancel the write.</param>
    /// <returns>A task that completes when the file is written.</returns>
    /// <remarks>The default implementation writes to the stream returned by <see cref="Create"/>.</remarks>
    async Task WriteAllBytesAsync(string path, byte[] contents, CancellationToken cancellationToken = default)
    {
        var stream = Create(path);
        await using (stream.ConfigureAwait(false))
        {
            await stream.WriteAsync(contents, cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Creates or overwrites a file with UTF-8 lines, each followed by a line terminator.
    /// </summary>
    /// <param name="path">The file path.</param>
    /// <param name="contents">The lines to write.</param>
    /// <param name="cancellationToken">A token used to cancel the write.</param>
    /// <returns>A task that completes when the file is written.</returns>
    /// <remarks>The default implementation writes to the stream returned by <see cref="Create"/>.</remarks>
    Task WriteAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default) =>
        WriteTextAsync(Create(path), contents, appendNewLines: true, cancellationToken);

    /// <summary>
    /// Appends UTF-8 text to a file, creating it when it does not exist.
    /// </summary>
    /// <param name="path">The file path.</param>
    /// <param name="contents">The text to append.</param>
    /// <param name="cancellationToken">A token used to cancel the write.</param>
    /// <returns>A task that completes when the text is appended.</returns>
    /// <remarks>The default implementation writes to <see cref="Open"/> with <see cref="FileMode.Append"/>.</remarks>
    Task AppendAllTextAsync(string path, string contents, CancellationToken cancellationToken = default) =>
        WriteTextAsync(Open(path, FileMode.Append, FileAccess.Write), [contents], appendNewLines: false, cancellationToken);

    /// <summary>
    /// Appends UTF-8 lines to a file, creating it when it does not exist.
    /// </summary>
    /// <param name="path">The file path.</param>
    /// <param name="contents">The lines to append.</param>
    /// <param name="cancellationToken">A token used to cancel the write.</param>
    /// <returns>A task that completes when the lines are appended.</returns>
    /// <remarks>The default implementation writes to <see cref="Open"/> with <see cref="FileMode.Append"/>.</remarks>
    Task AppendAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default) =>
        WriteTextAsync(Open(path, FileMode.Append, FileAccess.Write), contents, appendNewLines: true, cancellationToken);

    /// <summary>
    /// Opens an existing file for reading.
    /// </summary>
    /// <param name="path">The file path.</param>
    /// <returns>A readable stream owned by the caller.</returns>
    /// <remarks>The default implementation calls <see cref="Open"/> with <see cref="FileMode.Open"/> and <see cref="FileAccess.Read"/>.</remarks>
    Stream OpenRead(string path) => Open(path, FileMode.Open, FileAccess.Read);

    /// <summary>
    /// Creates or truncates a file and opens it for reading and writing.
    /// </summary>
    /// <param name="path">The file path.</param>
    /// <returns>A writable stream owned by the caller.</returns>
    /// <remarks>The default implementation calls <see cref="Open"/> with <see cref="FileMode.Create"/> and <see cref="FileAccess.ReadWrite"/>.</remarks>
    Stream Create(string path) => Open(path, FileMode.Create, FileAccess.ReadWrite);

    /// <summary>
    /// Opens a file with the specified mode and access.
    /// </summary>
    /// <param name="path">The file path.</param>
    /// <param name="mode">How to open or create the file.</param>
    /// <param name="access">The access the returned stream permits.</param>
    /// <returns>A stream owned by the caller.</returns>
    Stream Open(string path, FileMode mode, FileAccess access);

    /// <summary>
    /// Deletes a file. Deleting a file that does not exist does nothing.
    /// </summary>
    /// <param name="path">The file path.</param>
    void DeleteFile(string path);

    /// <summary>
    /// Copies a file's contents.
    /// </summary>
    /// <param name="sourcePath">The file to copy.</param>
    /// <param name="destinationPath">The destination file.</param>
    /// <param name="overwrite">Whether an existing destination file is replaced.</param>
    /// <exception cref="IOException">The destination exists and <paramref name="overwrite"/> is <see langword="false"/>.</exception>
    /// <remarks>
    /// The default implementation copies between <see cref="OpenRead"/> and <see cref="Create"/> streams;
    /// it does not copy metadata.
    /// </remarks>
    void CopyFile(string sourcePath, string destinationPath, bool overwrite)
    {
        if (!overwrite && FileExists(destinationPath))
        {
            throw new IOException($"The file '{destinationPath}' already exists.");
        }

        using var source = OpenRead(sourcePath);
        using var destination = Create(destinationPath);
        source.CopyTo(destination);
    }

    /// <summary>
    /// Moves a file to a destination that must not exist.
    /// </summary>
    /// <param name="sourcePath">The file to move.</param>
    /// <param name="destinationPath">The destination file.</param>
    void MoveFile(string sourcePath, string destinationPath);

    /// <summary>
    /// Moves a file, optionally replacing an existing destination.
    /// </summary>
    /// <param name="sourcePath">The file to move.</param>
    /// <param name="destinationPath">The destination file.</param>
    /// <param name="overwrite">Whether an existing destination file is replaced.</param>
    /// <exception cref="NotSupportedException">
    /// The default implementation cannot replace an existing destination atomically.
    /// </exception>
    void MoveFile(string sourcePath, string destinationPath, bool overwrite)
    {
        if (overwrite && FileExists(destinationPath))
        {
            throw new NotSupportedException(
                "This file system provider does not support atomic overwrite moves.");
        }

        MoveFile(sourcePath, destinationPath);
    }

    /// <summary>
    /// Returns whether a file exists.
    /// </summary>
    /// <param name="path">The file path.</param>
    /// <returns><see langword="true"/> when the file exists.</returns>
    bool FileExists(string path);

    /// <summary>
    /// Creates a directory and any missing parent directories.
    /// </summary>
    /// <param name="path">The directory path.</param>
    void CreateDirectory(string path);

    /// <summary>
    /// Deletes a directory.
    /// </summary>
    /// <param name="path">The directory path.</param>
    /// <param name="recursive">Whether contents are deleted; otherwise the directory must be empty.</param>
    void DeleteDirectory(string path, bool recursive);

    /// <summary>
    /// Moves a directory and its contents to a destination that must not exist.
    /// </summary>
    /// <param name="sourcePath">The directory to move.</param>
    /// <param name="destinationPath">The destination directory.</param>
    void MoveDirectory(string sourcePath, string destinationPath);

    /// <summary>
    /// Returns whether a directory exists.
    /// </summary>
    /// <param name="path">The directory path.</param>
    /// <returns><see langword="true"/> when the directory exists.</returns>
    bool DirectoryExists(string path);

    /// <summary>
    /// Enumerates file paths in a directory.
    /// </summary>
    /// <param name="path">The directory path.</param>
    /// <param name="searchPattern">A file-name pattern supporting <c>*</c> and <c>?</c> wildcards.</param>
    /// <param name="searchOption">Whether subdirectories are searched.</param>
    /// <returns>The full paths of matching files.</returns>
    IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption);

    /// <summary>
    /// Enumerates directory paths in a directory.
    /// </summary>
    /// <param name="path">The directory path.</param>
    /// <param name="searchPattern">A directory-name pattern supporting <c>*</c> and <c>?</c> wildcards.</param>
    /// <param name="searchOption">Whether subdirectories are searched.</param>
    /// <returns>The full paths of matching directories.</returns>
    IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption);

    /// <summary>
    /// Gets the attributes of a file or directory.
    /// </summary>
    /// <param name="path">The file or directory path.</param>
    /// <returns>The entry's attributes.</returns>
    FileAttributes GetAttributes(string path);

    /// <summary>
    /// Sets the attributes of a file or directory.
    /// </summary>
    /// <param name="path">The file or directory path.</param>
    /// <param name="attributes">The attributes to apply.</param>
    void SetAttributes(string path, FileAttributes attributes);

    /// <summary>
    /// Gets the creation time of a file or directory.
    /// </summary>
    /// <param name="path">The file or directory path.</param>
    /// <returns>The creation time in UTC.</returns>
    DateTime GetCreationTimeUtc(string path);

    /// <summary>
    /// Sets the creation time of a file or directory.
    /// </summary>
    /// <param name="path">The file or directory path.</param>
    /// <param name="creationTimeUtc">The creation time in UTC.</param>
    void SetCreationTimeUtc(string path, DateTime creationTimeUtc);

    /// <summary>
    /// Gets the last write time of a file or directory.
    /// </summary>
    /// <param name="path">The file or directory path.</param>
    /// <returns>The last write time in UTC.</returns>
    DateTime GetLastWriteTimeUtc(string path);

    /// <summary>
    /// Sets the last write time of a file or directory.
    /// </summary>
    /// <param name="path">The file or directory path.</param>
    /// <param name="lastWriteTimeUtc">The last write time in UTC.</param>
    void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc);

    /// <summary>
    /// Gets the last access time of a file or directory.
    /// </summary>
    /// <param name="path">The file or directory path.</param>
    /// <returns>The last access time in UTC.</returns>
    DateTime GetLastAccessTimeUtc(string path);

    /// <summary>
    /// Sets the last access time of a file or directory.
    /// </summary>
    /// <param name="path">The file or directory path.</param>
    /// <param name="lastAccessTimeUtc">The last access time in UTC.</param>
    void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc);

    /// <summary>
    /// Gets the length of a file in bytes.
    /// </summary>
    /// <param name="path">The file path.</param>
    /// <returns>The file length in bytes.</returns>
    /// <remarks>The default implementation reads the length of the stream returned by <see cref="OpenRead"/>.</remarks>
    long GetFileLength(string path)
    {
        using var stream = OpenRead(path);
        return stream.Length;
    }

    /// <summary>
    /// Gets the directory used for temporary files.
    /// </summary>
    /// <returns>The temporary directory path.</returns>
    /// <remarks>The default implementation returns <see cref="System.IO.Path.GetTempPath"/>.</remarks>
    string GetTempPath() => Path.GetTempPath();

    /// <summary>
    /// Gets a random file or directory name.
    /// </summary>
    /// <returns>A random name.</returns>
    /// <remarks>The default implementation returns <see cref="System.IO.Path.GetRandomFileName"/>.</remarks>
    string GetRandomFileName() => Path.GetRandomFileName();

    /// <summary>
    /// Combines path segments.
    /// </summary>
    /// <param name="paths">The segments to combine.</param>
    /// <returns>The combined path.</returns>
    /// <remarks>The default implementation calls <see cref="System.IO.Path.Combine(string[])"/>.</remarks>
    string Combine(params string[] paths) => Path.Combine(paths);

    /// <summary>
    /// Gets a path relative to another path.
    /// </summary>
    /// <param name="relativeTo">The base path.</param>
    /// <param name="path">The path to make relative.</param>
    /// <returns>The relative path.</returns>
    /// <remarks>The default implementation calls <see cref="System.IO.Path.GetRelativePath(string, string)"/>.</remarks>
    string GetRelativePath(string relativeTo, string path) => Path.GetRelativePath(relativeTo, path);

    private static async Task WriteTextAsync(
        Stream stream,
        IEnumerable<string> contents,
        bool appendNewLines,
        CancellationToken cancellationToken)
    {
        await using (stream.ConfigureAwait(false))
        {
            var writer = new StreamWriter(stream, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            await using (writer.ConfigureAwait(false))
            {
                foreach (var content in contents)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await writer.WriteAsync(content.AsMemory(), cancellationToken).ConfigureAwait(false);
                    if (appendNewLines)
                    {
                        await writer.WriteLineAsync(ReadOnlyMemory<char>.Empty, cancellationToken)
                            .ConfigureAwait(false);
                    }
                }
            }
        }
    }
}
