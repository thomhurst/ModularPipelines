using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("config", "set")]
public record AzConfigSetOptions : AzOptions
{
    [CliFlag("--local")]
    public bool? Local { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? KEYVALUE { get; set; }
}