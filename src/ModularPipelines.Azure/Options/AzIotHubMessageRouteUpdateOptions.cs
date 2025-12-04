using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "hub", "message-route", "update")]
public record AzIotHubMessageRouteUpdateOptions(
[property: CliOption("--hub-name")] string HubName,
[property: CliOption("--rn")] string Rn
) : AzOptions
{
    [CliOption("--condition")]
    public string? Condition { get; set; }

    [CliOption("--en")]
    public string? En { get; set; }

    [CliFlag("--enabled")]
    public bool? Enabled { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--source-type")]
    public string? SourceType { get; set; }
}