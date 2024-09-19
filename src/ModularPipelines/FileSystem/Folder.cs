using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Microsoft.Extensions.Logging;
using ModularPipelines.JsonUtils;
using ModularPipelines.Logging;

namespace ModularPipelines.FileSystem;

public class Folder : IEquatable<Folder>
{
    [JsonIgnore]
    private readonly DirectoryInfo _directoryInfo;

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
        OriginalPath = Path;
    }

    public bool Exists => DirectoryInfo.Exists;

    public bool Hidden => (DirectoryInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;

    public string Name => DirectoryInfo.Name;

    [JsonConverter(typeof(FolderPathJsonConverter))]
    public Folder? Parent => DirectoryInfo.Parent;

    public string Path => DirectoryInfo.FullName;

    public string OriginalPath { get; }

    public FileAttributes Attributes
    {
        get => DirectoryInfo.Attributes;
        set => DirectoryInfo.Attributes = value;
    }

    [JsonConverter(typeof(FolderPathJsonConverter))]
    public Folder Root
    {
        get
        {
            if (DirectoryInfo.Root.FullName == Path)
            {
                return this;
            }
            
            return DirectoryInfo.Root;
        }
    }

    public DateTimeOffset CreationTime => DirectoryInfo.CreationTime;

    public DateTimeOffset LastWriteTimeUtc => DirectoryInfo.LastWriteTimeUtc;

    public string Extension => DirectoryInfo.Extension;

    public Folder Create()
    {
        ModuleLogger.Current.LogInformation("Creating Folder: {Path}", this);
        
        Directory.CreateDirectory(Path);
        return this;
    }

    public void Delete()
    {
        ModuleLogger.Current.LogInformation("Deleting Folder: {Path}", this);
        
        DirectoryInfo.Delete(true);
    }

    public void Clean()
    {
        ModuleLogger.Current.LogInformation("Cleaning Folder: {Path}", this);
        
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
        
        ModuleLogger.Current.LogInformation("Copying Folder: {Source} > {Destination}", this, targetPath);

        return new Folder(targetPath);
    }

    public Folder MoveTo(string path)
    {
        ModuleLogger.Current.LogInformation("Moving Folder: {Source} > {Destination}", this, path);

        DirectoryInfo.MoveTo(path);
        return this;
    }

    public Folder GetFolder(string name)
    {
        var directoryInfo = new DirectoryInfo(System.IO.Path.Combine(Path, name));
        
        ModuleLogger.Current.LogInformation("Getting Folder: {Path}", directoryInfo.FullName);
        
        return directoryInfo;
    }

    public Folder CreateFolder(string name)
    {
        var folder = GetFolder(name).Create();
        
        ModuleLogger.Current.LogInformation("Creating Folder: {Path}", folder);
        
        return folder;
    }

    public File GetFile(string name)
    {
        var fileInfo = new FileInfo(System.IO.Path.Combine(Path, name));
        
        ModuleLogger.Current.LogInformation("Getting File: {Path}", fileInfo.FullName);
        
        return fileInfo;
    }

    public File CreateFile(string name)
    {
        return GetFile(name).Create();
    }

    public IEnumerable<Folder> GetFolders(Func<Folder, bool> predicate, [CallerArgumentExpression("predicate")] string predicateExpression = "") => GetFolders(predicate, _ => false, predicateExpression);

    public IEnumerable<File> GetFiles(Func<File, bool> predicate, [CallerArgumentExpression("predicate")] string predicateExpression = "") => GetFiles(predicate, _ => false, predicateExpression);

    public IEnumerable<Folder> GetFolders(Func<Folder, bool> predicate, Func<Folder, bool> exclusionFilters, [CallerArgumentExpression("predicate")] string predicateExpression = "")
    {
        ModuleLogger.Current.LogInformation("Searching Folders in: {Path} > {Expression}", this, predicateExpression);

        return SafeWalk.EnumerateFolders(this, exclusionFilters)
            .Select(x => new Folder(x))
            .Distinct()
            .Where(predicate);
    }

    public IEnumerable<File> GetFiles(Func<File, bool> predicate, Func<Folder, bool> directoryExclusionFilters, [CallerArgumentExpression("predicate")] string predicateExpression = "")
    {
        ModuleLogger.Current.LogInformation("Searching Files in: {Path} > {Expression}", this, predicateExpression);
        
        return SafeWalk.EnumerateFiles(this, directoryExclusionFilters)
            .Select(x => new File(x))
            .Distinct()
            .Where(predicate);
    }

    public IEnumerable<File> GetFiles(string globPattern)
    {
        ModuleLogger.Current.LogInformation("Searching Files in: {Path} > {Glob}", this, globPattern);

        return new Matcher(StringComparison.OrdinalIgnoreCase)
            .AddInclude(globPattern)
            .Execute(new DirectoryInfoWrapper(DirectoryInfo))
            .Files
            .Select(x => new File(System.IO.Path.Combine(this, x.Path)))
            .Distinct();
    }

    public File? FindFile(Func<File, bool> predicate, [CallerArgumentExpression("predicate")] string predicateExpression = "") => FindFile(predicate, _ => false, predicateExpression);

    public Folder? FindFolder(Func<Folder, bool> predicate, [CallerArgumentExpression("predicate")] string predicateExpression = "") => FindFolder(predicate, _ => false, predicateExpression);

    public File? FindFile(Func<File, bool> predicate, Func<Folder, bool> directoryExclusionFilters, [CallerArgumentExpression("predicate")] string predicateExpression = "") => GetFiles(predicate, directoryExclusionFilters, predicateExpression).FirstOrDefault();

    public Folder? FindFolder(Func<Folder, bool> predicate, Func<Folder, bool> directoryExclusionFilters, [CallerArgumentExpression("predicate")] string predicateExpression = "") => GetFolders(predicate, directoryExclusionFilters, predicateExpression).FirstOrDefault();

    public IEnumerable<File> ListFiles()
    {
        return DirectoryInfo.EnumerateFiles("*", SearchOption.TopDirectoryOnly)
            .Select(x => new File(x))
            .Distinct();
    }

    public IEnumerable<Folder> ListFolders()
    {
        return DirectoryInfo.EnumerateDirectories("*", SearchOption.TopDirectoryOnly)
            .Select(x => new Folder(x))
            .Distinct();
    }

    public static Folder CreateTemporaryFolder()
    {
        var tempDirectory = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName().Replace(".", string.Empty));
        Directory.CreateDirectory(tempDirectory);
        
        ModuleLogger.Current.LogInformation("Creating Temporary Folder: {Path}", tempDirectory);
        
        return tempDirectory!;
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

    /// <inheritdoc/>
    public override string ToString()
    {
        return Path;
    }

    /// <inheritdoc/>
    public bool Equals(Folder? other)
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

        return obj is Folder other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Path.GetHashCode();
    }

    public static bool operator ==(Folder? left, Folder? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Folder? left, Folder? right)
    {
        return !Equals(left, right);
    }
}