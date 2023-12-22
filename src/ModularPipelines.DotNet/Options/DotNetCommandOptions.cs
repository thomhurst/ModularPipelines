using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetCommandOptions : DotNetOptions
{
    public IEnumerable<string>? Command { get; init; }
}