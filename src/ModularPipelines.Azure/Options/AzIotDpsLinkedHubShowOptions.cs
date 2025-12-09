using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "dps", "linked-hub", "show")]
public record AzIotDpsLinkedHubShowOptions(
[property: CliOption("--dps-name")] string DpsName,
[property: CliOption("--linked-hub")] string LinkedHub
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}