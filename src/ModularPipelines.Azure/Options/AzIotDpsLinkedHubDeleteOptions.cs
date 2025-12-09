using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "dps", "linked-hub", "delete")]
public record AzIotDpsLinkedHubDeleteOptions(
[property: CliOption("--dps-name")] string DpsName,
[property: CliOption("--linked-hub")] string LinkedHub
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}