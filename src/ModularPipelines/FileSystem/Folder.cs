using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.FileSystem;

public class Folder
{
    private DirectoryInfo _directoryInfo;

    private DirectoryInfo DirectoryInfo
    {
        get
        {
            _directoryInfo.Refresh();
            return _directoryInfo;
        }
    }

    public Folder(string path) : this(new DirectoryInfo(path))
    {
    }

    internal Folder(DirectoryInfo directoryInfo)
    {
        _directoryInfo = directoryInfo;
    }

    public bool Exists => DirectoryInfo.Exists;

    public bool Hidden => (DirectoryInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;

    public string Name => DirectoryInfo.Name;

    public Folder? Parent => DirectoryInfo.Parent;

    public string Path => DirectoryInfo.FullName;

    public FileAttributes Attributes => DirectoryInfo.Attributes;

    public Folder Root => DirectoryInfo.Root;

    public DateTime CreationTime => DirectoryInfo.CreationTime;

    public DateTime LastWriteTimeUtc => DirectoryInfo.LastWriteTimeUtc;

    public string Extension => DirectoryInfo.Extension;

    public Folder Create()
    {
        Directory.CreateDirectory(Path);
        return this;
    }

    public void Delete() => DirectoryInfo.Delete(true);

    public void Clean()
    {
        foreach (var directory in DirectoryInfo.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
        {
            directory.Delete(true);
        }

        foreach (var file in DirectoryInfo.EnumerateFiles("*", SearchOption.TopDirectoryOnly))
        {
            file.Delete();
        }
    }

    public Folder CopyTo(string targetPath)
    {
        Directory.CreateDirectory(targetPath);
        
        foreach (var dirPath in Directory.EnumerateDirectories(this, "*", SearchOption.AllDirectories))
        {
            Directory.CreateDirectory(dirPath.Replace(this, targetPath));
        }

        foreach (var newPath in Directory.EnumerateFiles(this, "*", SearchOption.AllDirectories))
        {
            System.IO.File.Copy(newPath, newPath.Replace(this, targetPath), true);
        }

        return new Folder(targetPath);
    }

    public Folder MoveTo(string path)
    {
        var thisOriginalPath = Path;
        
        DirectoryInfo.MoveTo(path);

        _directoryInfo = new DirectoryInfo(thisOriginalPath);
        
        return new Folder(path);
    }

    public Folder GetFolder(string name) => new DirectoryInfo(System.IO.Path.Combine(Path, name));

    public File GetFile(string name) => new FileInfo(System.IO.Path.Combine(Path, name));

    public IEnumerable<Folder> GetFolders(Func<Folder, bool> predicate) => DirectoryInfo.EnumerateDirectories("*", SearchOption.AllDirectories)
        .Select(x => new Folder(x))
        .Where(predicate);

    public IEnumerable<File> GetFiles(Func<File, bool> predicate) => DirectoryInfo.EnumerateFiles("*", SearchOption.AllDirectories)
        .Select(x => new File(x))
        .Where(predicate);

    public File? FindFile(Func<File, bool> predicate) => GetFiles(predicate).FirstOrDefault();
    public Folder? FindFolder(Func<Folder, bool> predicate) => GetFolders(predicate).FirstOrDefault();

    public IEnumerable<File> ListFiles()
    {
        return DirectoryInfo.EnumerateFiles("*", SearchOption.TopDirectoryOnly)
            .Select(x => new File(x));
    }
    
    public IEnumerable<Folder> ListFolders()
    {
        return DirectoryInfo.EnumerateDirectories("*", SearchOption.TopDirectoryOnly)
            .Select(x => new Folder(x));
    }
    
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

    public static implicit operator string(Folder folder)
    {
        return folder.Path;
    }

    public override string ToString()
    {
        return Path;
    }
}
