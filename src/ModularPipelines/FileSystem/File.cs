using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.FileSystem;

public class File
{
    private readonly FileInfo _fileInfo;

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
    public bool Exists => _fileInfo.Exists;

    public bool Hidden => (_fileInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;

    /// <inheritdoc cref="FileSystemInfo.Name"/>>
    public string Name => _fileInfo.Name;

    /// <inheritdoc cref="System.IO.Path.GetFileNameWithoutExtension(System.ReadOnlySpan{char})"/>>
    public string NameWithoutExtension => System.IO.Path.GetFileNameWithoutExtension(this);

    /// <inheritdoc cref="FileInfo.Directory"/>>
    public Folder? Folder => _fileInfo.Directory;

    /// <inheritdoc cref="FileSystemInfo.FullName"/>>
    public string Path => _fileInfo.FullName;

    /// <inheritdoc cref="FileSystemInfo.Attributes"/>>
    public FileAttributes Attributes => _fileInfo.Attributes;

    /// <inheritdoc cref="FileInfo.IsReadOnly"/>>
    public bool IsReadOnly => _fileInfo.IsReadOnly;

    /// <inheritdoc cref="FileSystemInfo.CreationTime"/>>
    public DateTime CreationTime => _fileInfo.CreationTime;

    public DateTime LastWriteTimeUtc => _fileInfo.LastWriteTimeUtc;

    /// <inheritdoc cref="FileSystemInfo.Extension"/>>
    public string Extension => _fileInfo.Extension;

    /// <inheritdoc cref="FileInfo.Delete"/>>
    public void Delete() => _fileInfo.Delete();

    /// <inheritdoc cref="FileInfo.MoveTo(string)"/>>
    public void MoveTo(string path) => _fileInfo.MoveTo(path);

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
