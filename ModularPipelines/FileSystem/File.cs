namespace ModularPipelines.FileSystem;

public class File
{
    private readonly FileInfo _fileInfo;

    internal File(FileInfo fileInfo)
    {
        _fileInfo = fileInfo;
    }

    public bool Exists => _fileInfo.Exists;

    public string Name => _fileInfo.Name;

    public Folder? Folder => _fileInfo.Directory == null ? null : new Folder(_fileInfo.Directory);

    public string Path => _fileInfo.FullName;

    public FileAttributes Attributes => _fileInfo.Attributes;

    public bool IsReadOnly => _fileInfo.IsReadOnly;

    public DateTime CreationTime => _fileInfo.CreationTime;
    
    public DateTime LastWriteTimeUtc => _fileInfo.LastWriteTimeUtc;

    public string Extension => _fileInfo.Extension;
    
    public void Delete() => _fileInfo.Delete();
    
    public void MoveTo(string path) => _fileInfo.MoveTo(path);
}