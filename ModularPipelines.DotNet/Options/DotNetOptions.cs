using ModularPipelines.Command.Options;

namespace ModularPipelines.DotNet.Options;

public record DotNetOptions : CommandEnvironmentOptions
{
    public string? TargetPath { get; init; }
    
    public IEnumerable<string>? ExtraArguments { get; init; }
    
    public Configuration? Configuration { get; init; }
}