using ModularPipelines.FileSystem;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Context;

public class FileSystemContext : IFileSystemContext
{
    public void DeleteFile(string filePath) => System.IO.File.Delete(filePath);
    
    public void DeleteFolder(string folderPath) => Directory.Delete(folderPath, true);

    public void CopyFile(string filePath, string destinationFilePath) => System.IO.File.Copy(filePath, destinationFilePath);

    public void CopyFolder(string folderPath, string destinationFolder)
    {
        foreach (var innerFolder in Directory.EnumerateDirectories(folderPath, "*", SearchOption.AllDirectories))
        {
            Directory.CreateDirectory(innerFolder.Replace(folderPath, destinationFolder));
        }
        
        foreach (var newPath in Directory.EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories))
        {
            System.IO.File.Copy(newPath, newPath.Replace(folderPath, destinationFolder), true);
        }
    }

    public void MoveFile(string filePath, string destinationFilePath) => System.IO.File.Move(filePath, destinationFilePath);
    
    public void MoveFolder(string filePath, string destinationFilePath) => Directory.Move(filePath, destinationFilePath);

    public bool FileExists(string filePath) => System.IO.File.Exists(filePath);
    
    public bool FolderExists(string filePath) => Directory.Exists(filePath);
    
    public FileAttributes GetFileAttributes(string filePath) => System.IO.File.GetAttributes(filePath);
    public void SetFileAttributes(string filepath, FileAttributes attributes) => System.IO.File.SetAttributes(filepath, attributes);
    
    public IEnumerable<File> GetFiles(string folderPath, SearchOption searchOption)
    {
        return Directory.EnumerateFiles(folderPath, "*", searchOption).Select(filePath => new File(new FileInfo(filePath)));
    }

    public IEnumerable<File> GetFiles(string folderPath, SearchOption searchOption, Func<File, bool> predicate)
    {
        return GetFiles(folderPath, searchOption).Where(predicate);
    }

    public IEnumerable<Folder> GetFolders(string folderPath, SearchOption searchOption)
    {
        return Directory.EnumerateDirectories(folderPath, "*", searchOption).Select(path => new Folder(new DirectoryInfo(path)));
    }

    public IEnumerable<Folder> GetFolders(string folderPath, SearchOption searchOption, Func<Folder, bool> predicate)
    {
        return GetFolders(folderPath, searchOption).Where(predicate);
    }

    public Folder GetFolder(Environment.SpecialFolder specialFolder)
    {
        return new Folder(new DirectoryInfo(Environment.GetFolderPath(specialFolder)));
    }
}