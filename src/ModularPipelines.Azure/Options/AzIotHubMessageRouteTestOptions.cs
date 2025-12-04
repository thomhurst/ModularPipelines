using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "hub", "message-route", "test")]
public record AzIotHubMessageRouteTestOptions(
[property: CliOption("--hub-name")] string HubName
) : AzOptions
{
    [CliOption("--ap")]
    public string? Ap { get; set; }

    [CliOption("--body")]
    public string? Body { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--rn")]
    public string? Rn { get; set; }

    [CliOption("--source-type")]
    public string? SourceType { get; set; }

    [CliOption("--sp")]
    public string? Sp { get; set; }
}