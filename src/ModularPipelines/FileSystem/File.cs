using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ModularPipelines.FileSystem;

public class File : IEquatable<File>
{
    [JsonIgnore]
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
    public Task<string> ReadAsync(CancellationToken cancellationToken = default)
    {
        return System.IO.File.ReadAllTextAsync(Path, cancellationToken);
    }

    public Task<string[]> ReadLinesAsync(CancellationToken cancellationToken = default)
    {
        return System.IO.File.ReadAllLinesAsync(Path, cancellationToken);
    }

    public Task<byte[]> ReadBytesAsync(CancellationToken cancellationToken = default)
    {
        return System.IO.File.ReadAllBytesAsync(Path, cancellationToken);
    }

    public FileStream GetStream(FileAccess fileAccess = FileAccess.ReadWrite)
    {
        return System.IO.File.Open(Path, FileMode.OpenOrCreate, fileAccess);
    }

    public Task WriteAsync(string contents, CancellationToken cancellationToken = default)
    {
        return System.IO.File.WriteAllTextAsync(Path, contents, cancellationToken);
    }

    public Task WriteAsync(byte[] contents, CancellationToken cancellationToken = default)
    {
        return System.IO.File.WriteAllBytesAsync(Path, contents, cancellationToken);
    }

    public Task WriteAsync(IEnumerable<string> contents, CancellationToken cancellationToken = default)
    {
        return System.IO.File.WriteAllLinesAsync(Path, contents, cancellationToken);
    }

    public async Task WriteAsync(ReadOnlyMemory<byte> contents, CancellationToken cancellationToken = default)
    {
        await using var fileStream = System.IO.File.Create(Path);
        await fileStream.WriteAsync(contents, cancellationToken);
    }

    public async Task WriteAsync(Stream contents, CancellationToken cancellationToken = default)
    {
        await using var fileStream = System.IO.File.Create(Path);

        if (contents.CanSeek)
        {
            contents.Position = 0;
        }

        await contents.CopyToAsync(fileStream, cancellationToken);
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
    public DateTimeOffset CreationTime => FileInfo.CreationTime;

    public DateTimeOffset LastWriteTimeUtc => FileInfo.LastWriteTimeUtc;

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

    /// <inheritdoc cref="FileInfo.MoveTo(string)"/>>
    public File MoveTo(Folder folder)
    {
        folder.Create();
        return MoveTo(System.IO.Path.Combine(folder.Path, Name));
    }

    /// <inheritdoc cref="FileInfo.CopyTo(string)"/>>
    public File CopyTo(string path) => FileInfo.CopyTo(path);
    
    public File CopyTo(Folder folder)
    {
        folder.Create();
        return CopyTo(System.IO.Path.Combine(folder.Path, Name));
    }

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

    /// <inheritdoc/>
    public override string ToString()
    {
        return Path;
    }

    /// <inheritdoc/>
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

        if (OperatingSystem.IsWindows())
        {
            return string.Equals(Path, other.Path, StringComparison.OrdinalIgnoreCase);
        }

        return Path == other.Path;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj is File other && Equals(other);
    }

    /// <inheritdoc/>
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