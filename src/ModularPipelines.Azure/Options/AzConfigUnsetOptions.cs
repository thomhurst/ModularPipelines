using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("config", "unset")]
public record AzConfigUnsetOptions : AzOptions
{
    [CliFlag("--local")]
    public bool? Local { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Key { get; set; }
}