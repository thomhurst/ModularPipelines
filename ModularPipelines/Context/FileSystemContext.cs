namespace ModularPipelines.Context;

public class FileSystemContext : IFileSystemContext
{
    public void DeleteFile(string filePath) => File.Delete(filePath);
    
    public void DeleteFolder(string folderPath) => Directory.Delete(folderPath, true);

    public void CopyFile(string filePath, string destinationFilePath) => File.Copy(filePath, destinationFilePath);

    public void CopyFolder(string folderPath, string destinationFolder)
    {
        foreach (var innerFolder in Directory.EnumerateDirectories(folderPath, "*", SearchOption.AllDirectories))
        {
            Directory.CreateDirectory(innerFolder.Replace(folderPath, destinationFolder));
        }
        
        foreach (var newPath in Directory.EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories))
        {
            File.Copy(newPath, newPath.Replace(folderPath, destinationFolder), true);
        }
    }

    public void MoveFile(string filePath, string destinationFilePath) => File.Move(filePath, destinationFilePath);
    
    public void MoveFolder(string filePath, string destinationFilePath) => Directory.Move(filePath, destinationFilePath);

    public bool FileExists(string filePath) => File.Exists(filePath);
    
    public bool FolderExists(string filePath) => Directory.Exists(filePath);
    
    public FileAttributes GetFileAttributes(string filePath) => File.GetAttributes(filePath);
    public void SetFileAttributes(string filepath, FileAttributes attributes) => File.SetAttributes(filepath, attributes);
    
    public IEnumerable<FileInfo> GetFiles(string folderPath, SearchOption searchOption)
    {
        return Directory.EnumerateFiles(folderPath, "*", searchOption).Select(filePath => new FileInfo(filePath));
    }

    public IEnumerable<FileInfo> GetFiles(string folderPath, SearchOption searchOption, Func<FileInfo, bool> predicate)
    {
        return GetFiles(folderPath, searchOption).Where(predicate);
    }

    public IEnumerable<DirectoryInfo> GetFolders(string folderPath, SearchOption searchOption)
    {
        return Directory.EnumerateDirectories(folderPath, "*", searchOption).Select(path => new DirectoryInfo(path));
    }

    public IEnumerable<DirectoryInfo> GetFolders(string folderPath, SearchOption searchOption, Func<DirectoryInfo, bool> predicate)
    {
        return GetFolders(folderPath, searchOption).Where(predicate);
    }

    public DirectoryInfo GetFolder(Environment.SpecialFolder specialFolder)
    {
        return new DirectoryInfo(Environment.GetFolderPath(specialFolder));
    }
}