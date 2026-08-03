using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("-c")]
public record NpxCOptions : NpxOptions
{
    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public virtual string? Cmd { get; set; }
}
