using ModularPipelines.FileSystem;

namespace ModularPipelines.Context;

public interface IEnvironmentContext
{
    string EnvironmentName { get; }
    OperatingSystem OperatingSystem { get; }
    bool Is64BitOperatingSystem { get; }
    Folder WorkingDirectory { get; set; }
    Folder ContentDirectory { get; set; }

    Folder? GitRootDirectory { get; set; }

    Folder GetFolder(Environment.SpecialFolder specialFolder) => new(new DirectoryInfo(Environment.GetFolderPath(specialFolder)));
    
    string? GetEnvironmentVariable(string name);
    
    IDictionary<string, string> GetEnvironmentVariables();
}