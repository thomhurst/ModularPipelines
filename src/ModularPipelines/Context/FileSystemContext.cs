using ModularPipelines.FileSystem;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Context;

internal class FileSystemContext : IFileSystemContext
{
    public void DeleteFile(File file) => file.Delete();

    public void DeleteFolder(Folder folder) => folder.Delete();

    public File CopyFile(File file, string destinationFilePath) => file.CopyTo(destinationFilePath);

    public Folder CopyFolder(Folder folder, string destinationFolder) => folder.CopyTo(destinationFolder);

    public void MoveFile(File file, string destinationFilePath) => file.MoveTo(destinationFilePath);

    public void MoveFolder(Folder folder, string destinationFolderPath) => folder.MoveTo(destinationFolderPath);

    public bool FileExists(File file) => file.Exists;

    public bool FolderExists(Folder folder) => folder.Exists;

    public FileAttributes GetFileAttributes(File file) => file.Attributes;

    public void SetFileAttributes(File file, FileAttributes attributes) => file.Attributes = attributes;

    public FileAttributes GetFolderAttributes(Folder folder) => folder.Attributes;

    public void SetFolderAttributes(Folder folder, FileAttributes attributes) => folder.Attributes = attributes;

    public File GetFile(string filePath) => new(filePath);

    public IEnumerable<File> GetFiles(Folder rootFolder, Func<File, bool> predicate)
    {
        return rootFolder.GetFiles(predicate);
    }

    public IEnumerable<Folder> GetFolders(Folder rootFolder, Func<Folder, bool> predicate)
    {
        return rootFolder.GetFolders(predicate);
    }

    public Folder GetFolder(string path) => new DirectoryInfo(path);

    public Folder GetFolder(Environment.SpecialFolder specialFolder)
    {
        return new Folder(new DirectoryInfo(Environment.GetFolderPath(specialFolder)));
    }

    public Folder CreateTemporaryFolder()
    {
        return Folder.CreateTemporaryFolder();
    }

    public string GetNewTemporaryFilePath()
    {
        return File.GetNewTemporaryFilePath();
    }
}
