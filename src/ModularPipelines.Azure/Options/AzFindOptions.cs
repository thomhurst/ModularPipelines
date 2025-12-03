using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("find")]
public record AzFindOptions : AzOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? CLITERM { get; set; }
}