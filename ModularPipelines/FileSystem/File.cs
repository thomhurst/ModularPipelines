using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.FileSystem;

public class File
{
    private readonly FileInfo _fileInfo;

    public File( string path ) : this( new FileInfo( path ) )
    {
    }

    internal File( FileInfo fileInfo )
    {
        _fileInfo = fileInfo;
    }

    public Task<string> ReadAsync()
    {
        return System.IO.File.ReadAllTextAsync( Path );
    }

    public bool Exists => _fileInfo.Exists;

    public bool Hidden => (_fileInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;

    public string Name => _fileInfo.Name;

    public Folder? Folder => _fileInfo.Directory;

    public string Path => _fileInfo.FullName;

    public FileAttributes Attributes => _fileInfo.Attributes;

    public bool IsReadOnly => _fileInfo.IsReadOnly;

    public DateTime CreationTime => _fileInfo.CreationTime;

    public DateTime LastWriteTimeUtc => _fileInfo.LastWriteTimeUtc;

    public string Extension => _fileInfo.Extension;

    public void Delete() => _fileInfo.Delete();

    public void MoveTo( string path ) => _fileInfo.MoveTo( path );

    public static implicit operator File?( string? path )
    {
        if (string.IsNullOrEmpty( path ))
        {
            return null;
        }

        return new FileInfo( path );
    }

    [return: NotNullIfNotNull( "fileInfo" )]
    public static implicit operator File?( FileInfo? fileInfo )
    {
        if (fileInfo == null)
        {
            return null;
        }

        return new File( fileInfo );
    }

    public static implicit operator string( File file )
    {
        return file.Path;
    }

    public override string ToString()
    {
        return Path;
    }
}
