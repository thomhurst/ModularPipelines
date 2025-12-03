using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "hub", "message-route", "create")]
public record AzIotHubMessageRouteCreateOptions(
[property: CliOption("--en")] string En,
[property: CliOption("--hub-name")] string HubName,
[property: CliOption("--rn")] string Rn,
[property: CliOption("--source-type")] string SourceType
) : AzOptions
{
    [CliFlag("--condition")]
    public bool? Condition { get; set; }

    [CliFlag("--enabled")]
    public bool? Enabled { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}