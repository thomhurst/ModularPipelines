using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("pipelines", "pool", "show")]
public record AzPipelinesPoolShowOptions(
[property: CliOption("--id")] string Id
) : AzOptions
{
    [CliOption("--action")]
    public string? Action { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }
}