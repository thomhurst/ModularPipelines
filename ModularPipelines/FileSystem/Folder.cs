using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.FileSystem;

public class Folder
{
    private readonly DirectoryInfo _directoryInfo;

    internal Folder(DirectoryInfo directoryInfo)
    {
        _directoryInfo = directoryInfo;
    }

    public bool Exists => _directoryInfo.Exists;

    public string Name => _directoryInfo.Name;

    public Folder? Parent => _directoryInfo.Parent;

    public string Path => _directoryInfo.FullName;

    public FileAttributes Attributes => _directoryInfo.Attributes;

    public Folder Root => _directoryInfo.Root;

    public DateTime CreationTime => _directoryInfo.CreationTime;
    
    public DateTime LastWriteTimeUtc => _directoryInfo.LastWriteTimeUtc;

    public string Extension => _directoryInfo.Extension;
    
    public void Delete() => _directoryInfo.Delete();
    
    public void MoveTo(string path) => _directoryInfo.MoveTo(path);

    public IEnumerable<Folder> GetFolders(Func<Folder,bool> predicate) => _directoryInfo.EnumerateDirectories("*", SearchOption.AllDirectories)
        .Select(x => new Folder(x))
        .Where(predicate);
    
    public IEnumerable<File> GetFiles(Func<File,bool> predicate) => _directoryInfo.EnumerateFiles("*", SearchOption.AllDirectories)
        .Select(x => new File(x))
        .Where(predicate);
    
    public static implicit operator Folder?(string? path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return null;
        }
        
        return new DirectoryInfo(path);
    }

    [return: NotNullIfNotNull("directoryInfo")]
    public static implicit operator Folder?(DirectoryInfo? directoryInfo)
    {
        if (directoryInfo == null)
        {
            return null;
        }
        
        return new Folder(directoryInfo);
    }
}