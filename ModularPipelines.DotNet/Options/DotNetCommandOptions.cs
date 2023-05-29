namespace ModularPipelines.DotNet.Options;

public record DotNetCommandOptions : DotNetOptions 
{
    public IEnumerable<string>? Command { get; init; }
}