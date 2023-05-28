namespace ModularPipelines.DotNet.Options;

public record MultiDotNetModuleOptions
{
    public IEnumerable<string>? Command { get; init; }
    public string? WorkingDirectory { get; init; }
    public Func<string, bool>? ProjectsToInclude { get; init; }
    public Configuration Configuration { get; init; } = Configuration.Debug;
    public IEnumerable<string>? ExtraArguments { get; init; }
    public IDictionary<string,string?>? EnvironmentVariables { get; init; }
    public ParallelOptions ParallelOptions { get; init; } = ParallelOptions.OneAtATime;
}