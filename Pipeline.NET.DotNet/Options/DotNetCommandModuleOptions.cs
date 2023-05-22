namespace Pipeline.NET.DotNet.Options;

public record DotNetModuleOptions
{
    public string? WorkingDirectory { get; init; }
    public string? ProjectOrSolutionPath { get; init; }
    
    public IEnumerable<string>? ExtraArguments { get; init; }
    public bool AssertSuccess { get; init; } = true;
    
    public IDictionary<string, string?>? EnvironmentVariables { get; init; }
}

public record DotNetCommandModuleOptions : DotNetModuleOptions 
{
    public string Command { get; init; }

}