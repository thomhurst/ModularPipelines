using ModularPipelines.FileSystem;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Context;

internal class FileSystemContext : IFileSystemContext
{
    private readonly IFileSystemProvider _provider;
    private readonly PipelineWorkingDirectory _workingDirectory;

    public FileSystemContext(
        IFileSystemProvider provider,
        PipelineWorkingDirectory workingDirectory)
    {
        _provider = provider;
        _workingDirectory = workingDirectory;
    }

    public void DeleteFile(File file) => file.Delete();

    public void DeleteFolder(Folder folder) => folder.Delete();

    public File CopyFile(File file, string destinationFilePath) => file.CopyTo(ResolvePath(destinationFilePath));

    public Folder CopyFolder(Folder folder, string destinationFolder) => folder.CopyTo(ResolvePath(destinationFolder));

    public void MoveFile(File file, string destinationFilePath) => file.MoveTo(ResolvePath(destinationFilePath));

    public void MoveFolder(Folder folder, string destinationFolderPath) => folder.MoveTo(ResolvePath(destinationFolderPath));

    public bool FileExists(File file) => file.Exists;

    public bool FolderExists(Folder folder) => folder.Exists;

    public FileAttributes GetFileAttributes(File file) => file.Attributes;

    public void SetFileAttributes(File file, FileAttributes attributes) => file.Attributes = attributes;

    public FileAttributes GetFolderAttributes(Folder folder) => folder.Attributes;

    public void SetFolderAttributes(Folder folder, FileAttributes attributes) => folder.Attributes = attributes;

    public File GetFile(string filePath) => new(ResolvePath(filePath), _provider);

    public IEnumerable<File> GetFiles(Folder rootFolder, Func<File, bool> predicate)
    {
        return rootFolder.GetFiles(predicate);
    }

    public IEnumerable<Folder> GetFolders(Folder rootFolder, Func<Folder, bool> predicate)
    {
        return rootFolder.GetFolders(predicate);
    }

    public Folder GetFolder(string path) => new(ResolvePath(path), _provider);

    public Folder GetFolder(Environment.SpecialFolder specialFolder)
    {
        return new Folder(Environment.GetFolderPath(specialFolder), _provider);
    }

    public Folder CreateTemporaryFolder()
    {
        var path = _provider.Combine(_provider.GetTempPath(), _provider.GetRandomFileName().Replace(".", string.Empty));
        _provider.CreateDirectory(path);
        return new Folder(path, _provider);
    }

    public string GetNewTemporaryFilePath()
    {
        return _provider.Combine(_provider.GetTempPath(), _provider.GetRandomFileName());
    }

    private string ResolvePath(string path) => _workingDirectory.ResolvePath(path);
}
