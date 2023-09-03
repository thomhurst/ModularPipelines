using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.FileSystem;

public class File
{
    private FileInfo _fileInfo;

    private FileInfo FileInfo
    {
        get
        {
            _fileInfo.Refresh();
            return _fileInfo;
        }
    }

    public File(string path) : this(new FileInfo(path))
    {
    }

    internal File(FileInfo fileInfo)
    {
        _fileInfo = fileInfo;
    }

    /// <inheritdoc cref="System.IO.File.ReadAllTextAsync(string,System.Text.Encoding,System.Threading.CancellationToken)"/>>
    public Task<string> ReadAsync()
    {
        return System.IO.File.ReadAllTextAsync(Path);
    }

    public Task WriteAsync(string contents)
    {
        return System.IO.File.WriteAllTextAsync(Path, contents);
    }

    /// <inheritdoc cref="FileSystemInfo.Exists"/>>
    public bool Exists => FileInfo.Exists;

    public bool Hidden => (FileInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;

    /// <inheritdoc cref="FileSystemInfo.Name"/>>
    public string Name => FileInfo.Name;

    /// <inheritdoc cref="System.IO.Path.GetFileNameWithoutExtension(System.ReadOnlySpan{char})"/>>
    public string NameWithoutExtension => System.IO.Path.GetFileNameWithoutExtension(this);

    /// <inheritdoc cref="FileInfo.Directory"/>>
    public Folder? Folder => FileInfo.Directory;

    /// <inheritdoc cref="FileSystemInfo.FullName"/>>
    public string Path => FileInfo.FullName;

    /// <inheritdoc cref="FileSystemInfo.Attributes"/>>
    public FileAttributes Attributes => FileInfo.Attributes;

    /// <inheritdoc cref="FileInfo.IsReadOnly"/>>
    public bool IsReadOnly => FileInfo.IsReadOnly;

    /// <inheritdoc cref="FileSystemInfo.CreationTime"/>>
    public DateTime CreationTime => FileInfo.CreationTime;

    public DateTime LastWriteTimeUtc => FileInfo.LastWriteTimeUtc;

    /// <inheritdoc cref="FileSystemInfo.Extension"/>>
    public string Extension => FileInfo.Extension;

    /// <inheritdoc cref="FileInfo.Delete"/>>
    public void Delete() => FileInfo.Delete();

    /// <inheritdoc cref="FileInfo.MoveTo(string)"/>>
    public File MoveTo(string path)
    {
        var thisOriginalPath = Path;
        
        FileInfo.MoveTo(path);

        _fileInfo = new FileInfo(thisOriginalPath);
        
        return new File(path);
    }

    /// <inheritdoc cref="FileInfo.CopyTo(string)"/>>
    public File CopyTo(string path) => FileInfo.CopyTo(path);
    
    public static implicit operator File?(string? path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return null;
        }

        return new FileInfo(path);
    }

    [return: NotNullIfNotNull("fileInfo")]
    public static implicit operator File?(FileInfo? fileInfo)
    {
        if (fileInfo == null)
        {
            return null;
        }

        return new File(fileInfo);
    }

    public static implicit operator string(File file)
    {
        return file.Path;
    }

    public override string ToString()
    {
        return Path;
    }
}
