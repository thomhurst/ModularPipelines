using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("config", "get")]
public record AzConfigGetOptions : AzOptions
{
    [CliFlag("--local")]
    public bool? Local { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Key { get; set; }
}