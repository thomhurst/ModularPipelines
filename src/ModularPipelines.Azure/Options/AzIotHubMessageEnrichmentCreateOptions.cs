using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "hub", "message-enrichment", "create")]
public record AzIotHubMessageEnrichmentCreateOptions(
[property: CliOption("--endpoints")] string Endpoints,
[property: CliOption("--key")] string Key,
[property: CliOption("--name")] string Name,
[property: CliOption("--value")] string Value
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}