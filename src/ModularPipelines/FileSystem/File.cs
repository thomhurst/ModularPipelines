using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.FileSystem;

public class File : IEquatable<File>
{
    private readonly FileInfo _fileInfo;

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
        OriginalPath = Path;
    }

    /// <inheritdoc cref="System.IO.File.ReadAllTextAsync(string,System.Text.Encoding,System.Threading.CancellationToken)"/>>
    public Task<string> ReadAsync()
    {
        return System.IO.File.ReadAllTextAsync(Path);
    }
    
    public Task<string[]> ReadLinesAsync()
    {
        return System.IO.File.ReadAllLinesAsync(Path);
    }

    public Task WriteAsync(string contents)
    {
        return System.IO.File.WriteAllTextAsync(Path, contents);
    }
    
    public Task WriteLinesAsync(IEnumerable<string> contents)
    {
        return System.IO.File.WriteAllLinesAsync(Path, contents);
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
    public string OriginalPath { get; }
    
    public File Create()
    {
        var fileStream = System.IO.File.Create(Path);
        fileStream.Dispose();
        return this;
    }

    /// <inheritdoc cref="FileSystemInfo.Attributes"/>>
    public FileAttributes Attributes
    {
        get { return FileInfo.Attributes; }
        set { FileInfo.Attributes = value; }
    }

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
        FileInfo.MoveTo(path);
        return this;
    }

    /// <inheritdoc cref="FileInfo.CopyTo(string)"/>>
    public File CopyTo(string path) => FileInfo.CopyTo(path);
    
    public static File GetNewTemporaryFilePath()
    {
        return System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName())!;
    }
    
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
    
    public bool Equals(File? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Path == other.Path;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj is File other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Path.GetHashCode();
    }

    public static bool operator ==(File? left, File? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(File? left, File? right)
    {
        return !Equals(left, right);
    }
}
