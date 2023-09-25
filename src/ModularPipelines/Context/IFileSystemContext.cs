using ModularPipelines.FileSystem;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Context;

public interface IFileSystemContext
{
    void DeleteFile(File file);

    void DeleteFolder(Folder folder);

    File CopyFile(File file, string destinationFilePath);

    Folder CopyFolder(Folder folder, string destinationFolder);

    void MoveFile(File file, string destinationFilePath);

    void MoveFolder(Folder folder, string destinationFolderPath);

    bool FileExists(File file);

    bool FolderExists(Folder folder);

    FileAttributes GetFileAttributes(File file);

    void SetFileAttributes(File file, FileAttributes attributes);

    FileAttributes GetFolderAttributes(Folder folder);

    void SetFolderAttributes(Folder folder, FileAttributes attributes);

    File GetFile(string filePath);

    IEnumerable<File> GetFiles(Folder rootFolder, Func<File, bool> predicate);

    IEnumerable<Folder> GetFolders(Folder rootFolder, Func<Folder, bool> predicate);

    Folder GetFolder(string path);

    Folder GetFolder(Environment.SpecialFolder specialFolder);

    Folder CreateTemporaryFolder();

    string GetNewTemporaryFilePath();
}
