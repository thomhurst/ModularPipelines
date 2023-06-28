using ModularPipelines.FileSystem;

namespace ModularPipelines.Context;

public interface IEnvironmentContext
{
    string EnvironmentName { get; }
    OperatingSystem OperatingSystem { get; }
    bool Is64BitOperatingSystem { get; }
    Folder WorkingDirectory { get; set; }
    Folder AppDomainDirectory { get; }
    Folder ContentDirectory { get; set; }

    Folder? GitRootDirectory { get; set; }

    Folder? GetFolder(Environment.SpecialFolder specialFolder);
    
    IEnvironmentVariables EnvironmentVariables { get; }
}