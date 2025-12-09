using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("find")]
public record AzFindOptions : AzOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? CLITERM { get; set; }
}