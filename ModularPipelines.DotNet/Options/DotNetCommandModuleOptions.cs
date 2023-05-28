namespace ModularPipelines.DotNet.Options;

public record DotNetCommandModuleOptions : DotNetModuleOptions 
{
    public IEnumerable<string>? Command { get; init; }
}