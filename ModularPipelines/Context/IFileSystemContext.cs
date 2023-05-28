namespace ModularPipelines.Context;

public interface IFileSystemContext
{
    void DeleteFile(string filePath);
    void DeleteFolder(string folderPath);

    void CopyFile(string filePath, string destinationFilePath);

    void CopyFolder(string folderPath, string destinationFolder);

    void MoveFile(string filePath, string destinationFilePath);

    void MoveFolder(string filePath, string destinationFilePath);

    bool FileExists(string filePath);
    bool FolderExists(string filePath);

    FileAttributes GetFileAttributes(string filePath);
    void SetFileAttributes(string filepath, FileAttributes attributes);

    IEnumerable<FileInfo> GetFiles(string folderPath, SearchOption searchOption);
    
    IEnumerable<FileInfo> GetFiles(string folderPath, SearchOption searchOption, Func<FileInfo, bool> predicate);
    
    IEnumerable<DirectoryInfo> GetFolders(string folderPath, SearchOption searchOption);
    
    IEnumerable<DirectoryInfo> GetFolders(string folderPath, SearchOption searchOption, Func<DirectoryInfo, bool> predicate);
}