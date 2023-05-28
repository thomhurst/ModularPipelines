namespace Pipeline.NET.DotNet.Options;

public record DotNetCommandModuleOptions : DotNetModuleOptions 
{
    public string? Command { get; init; }
}