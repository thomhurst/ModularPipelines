using ModularPipelines.FileSystem;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Context;

/// <summary>
/// Provides file system operations for working with files and folders.
/// </summary>
public interface IFileSystemContext
{
    /// <summary>
    /// Deletes a file from the file system.
    /// </summary>
    /// <param name="file">The file to delete.</param>
    void DeleteFile(File file);

    /// <summary>
    /// Deletes a folder and all its contents from the file system.
    /// </summary>
    /// <param name="folder">The folder to delete.</param>
    void DeleteFolder(Folder folder);

    /// <summary>
    /// Copies a file to the specified destination path.
    /// </summary>
    /// <param name="file">The source file to copy.</param>
    /// <param name="destinationFilePath">The destination path for the copied file.</param>
    /// <returns>A <see cref="File"/> instance representing the copied file.</returns>
    File CopyFile(File file, string destinationFilePath);

    /// <summary>
    /// Copies a folder and all its contents to the specified destination path.
    /// </summary>
    /// <param name="folder">The source folder to copy.</param>
    /// <param name="destinationFolder">The destination path for the copied folder.</param>
    /// <returns>A <see cref="Folder"/> instance representing the copied folder.</returns>
    Folder CopyFolder(Folder folder, string destinationFolder);

    /// <summary>
    /// Moves a file to the specified destination path.
    /// </summary>
    /// <param name="file">The file to move.</param>
    /// <param name="destinationFilePath">The destination path for the moved file.</param>
    void MoveFile(File file, string destinationFilePath);

    /// <summary>
    /// Moves a folder and all its contents to the specified destination path.
    /// </summary>
    /// <param name="folder">The folder to move.</param>
    /// <param name="destinationFolderPath">The destination path for the moved folder.</param>
    void MoveFolder(Folder folder, string destinationFolderPath);

    /// <summary>
    /// Checks whether a file exists at the specified path.
    /// </summary>
    /// <param name="file">The file to check.</param>
    /// <returns>True if the file exists; otherwise, false.</returns>
    bool FileExists(File file);

    /// <summary>
    /// Checks whether a folder exists at the specified path.
    /// </summary>
    /// <param name="folder">The folder to check.</param>
    /// <returns>True if the folder exists; otherwise, false.</returns>
    bool FolderExists(Folder folder);

    /// <summary>
    /// Gets the file system attributes of a file.
    /// </summary>
    /// <param name="file">The file to get attributes for.</param>
    /// <returns>The file attributes.</returns>
    FileAttributes GetFileAttributes(File file);

    /// <summary>
    /// Sets the file system attributes of a file.
    /// </summary>
    /// <param name="file">The file to set attributes on.</param>
    /// <param name="attributes">The attributes to set.</param>
    void SetFileAttributes(File file, FileAttributes attributes);

    /// <summary>
    /// Gets the file system attributes of a folder.
    /// </summary>
    /// <param name="folder">The folder to get attributes for.</param>
    /// <returns>The folder attributes.</returns>
    FileAttributes GetFolderAttributes(Folder folder);

    /// <summary>
    /// Sets the file system attributes of a folder.
    /// </summary>
    /// <param name="folder">The folder to set attributes on.</param>
    /// <param name="attributes">The attributes to set.</param>
    void SetFolderAttributes(Folder folder, FileAttributes attributes);

    /// <summary>
    /// Gets a <see cref="File"/> instance for the specified path.
    /// </summary>
    /// <param name="filePath">The path to the file.</param>
    /// <returns>A <see cref="File"/> instance representing the file.</returns>
    File GetFile(string filePath);

    /// <summary>
    /// Gets all files in a folder that match the specified predicate.
    /// </summary>
    /// <param name="rootFolder">The root folder to search in.</param>
    /// <param name="predicate">A function to filter files.</param>
    /// <returns>An enumerable of matching files.</returns>
    IEnumerable<File> GetFiles(Folder rootFolder, Func<File, bool> predicate);

    /// <summary>
    /// Gets all folders in a folder that match the specified predicate.
    /// </summary>
    /// <param name="rootFolder">The root folder to search in.</param>
    /// <param name="predicate">A function to filter folders.</param>
    /// <returns>An enumerable of matching folders.</returns>
    IEnumerable<Folder> GetFolders(Folder rootFolder, Func<Folder, bool> predicate);

    /// <summary>
    /// Gets a <see cref="Folder"/> instance for the specified path.
    /// </summary>
    /// <param name="path">The path to the folder.</param>
    /// <returns>A <see cref="Folder"/> instance representing the folder.</returns>
    Folder GetFolder(string path);

    /// <summary>
    /// Gets a <see cref="Folder"/> instance for the specified special folder.
    /// </summary>
    /// <param name="specialFolder">The special folder to get.</param>
    /// <returns>A <see cref="Folder"/> instance representing the special folder.</returns>
    Folder GetFolder(Environment.SpecialFolder specialFolder);

    /// <summary>
    /// Creates a new temporary folder in the system's temp directory.
    /// </summary>
    /// <returns>A <see cref="Folder"/> instance representing the created temporary folder.</returns>
    Folder CreateTemporaryFolder();

    /// <summary>
    /// Gets a new unique temporary file path.
    /// </summary>
    /// <returns>A path to a new temporary file (the file is not created).</returns>
    string GetNewTemporaryFilePath();
}