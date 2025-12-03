using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("webpubsub", "hub", "create")]
public record AzWebpubsubHubCreateOptions(
[property: CliOption("--hub-name")] string HubName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--allow-anonymous")]
    public bool? AllowAnonymous { get; set; }

    [CliOption("--event-handler")]
    public string? EventHandler { get; set; }
}