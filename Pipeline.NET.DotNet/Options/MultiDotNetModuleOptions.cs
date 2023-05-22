namespace Pipeline.NET.DotNet.Options;

public record MultiDotNetModuleOptions
{
    public string Command { get; init; }
    public string? WorkingDirectory { get; init; }
    public Func<string, bool>? ProjectsToInclude { get; init; }
    
    public IEnumerable<string>? ExtraArguments { get; init; }
    public bool AssertSuccess { get; init; } = true;
}