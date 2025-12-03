using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("npx", "-c")]
public record NpxCOptions : NpmOptions
{
    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string? Cmd { get; set; }
}