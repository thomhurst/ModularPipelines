using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("group", "delete")]
public record AzGroupDeleteOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliFlag("--force-deletion-types")]
    public bool? ForceDeletionTypes { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}