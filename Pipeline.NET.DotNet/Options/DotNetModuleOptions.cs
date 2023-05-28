namespace Pipeline.NET.DotNet.Options;

public record DotNetModuleOptions
{
    public string? WorkingDirectory { get; init; }
    
    public string? ProjectOrSolutionPath { get; init; }
    
    public IEnumerable<string>? ExtraArguments { get; init; }
    
    public Configuration Configuration { get; init; } = Configuration.Debug;
    
    public IDictionary<string, string?>? EnvironmentVariables { get; init; }
}