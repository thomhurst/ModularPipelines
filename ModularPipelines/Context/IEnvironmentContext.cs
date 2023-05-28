namespace ModularPipelines.Context;

public interface IEnvironmentContext
{
    OperatingSystem OperatingSystem { get; }
    bool Is64BitOperatingSystem { get; }
    DirectoryInfo WorkingDirectory { get; set; }
    DirectoryInfo? GitRootDirectory { get; set; }

    DirectoryInfo GetFolder(Environment.SpecialFolder specialFolder) => new(Environment.GetFolderPath(specialFolder));
    
    string? GetEnvironmentVariable(string name);
    
    IDictionary<string, string> GetEnvironmentVariables();
}