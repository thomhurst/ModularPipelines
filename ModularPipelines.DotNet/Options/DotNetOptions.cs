namespace ModularPipelines.DotNet.Options;

public record DotNetOptions
{
    public string? WorkingDirectory { get; init; }
    
    public string? TargetPath { get; init; }
    
    public IEnumerable<string>? ExtraArguments { get; init; }
    
    public Configuration? Configuration { get; init; }
    
    public IDictionary<string, string?>? EnvironmentVariables { get; init; }
}