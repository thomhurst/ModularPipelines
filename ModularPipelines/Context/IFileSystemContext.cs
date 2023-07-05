using ModularPipelines.FileSystem;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Context;

public interface IFileSystemContext
{
    void DeleteFile( string filePath );
    void DeleteFolder( string folderPath );

    void CopyFile( string filePath, string destinationFilePath );

    void CopyFolder( string folderPath, string destinationFolder );

    void MoveFile( string filePath, string destinationFilePath );

    void MoveFolder( string filePath, string destinationFilePath );

    bool FileExists( string filePath );
    bool FolderExists( string filePath );

    FileAttributes GetFileAttributes( string filePath );
    void SetFileAttributes( string filepath, FileAttributes attributes );

    File GetFile( string filePath );
    IEnumerable<File> GetFiles( string folderPath, SearchOption searchOption );

    IEnumerable<File> GetFiles( string folderPath, SearchOption searchOption, Func<File, bool> predicate );

    IEnumerable<Folder> GetFolders( string folderPath, SearchOption searchOption );

    IEnumerable<Folder> GetFolders( string folderPath, SearchOption searchOption, Func<Folder, bool> predicate );

    Folder GetFolder( string path );
    Folder GetFolder( Environment.SpecialFolder specialFolder );
}
