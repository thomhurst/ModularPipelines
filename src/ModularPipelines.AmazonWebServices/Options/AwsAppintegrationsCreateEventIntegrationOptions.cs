using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appintegrations", "create-event-integration")]
public record AwsAppintegrationsCreateEventIntegrationOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--event-filter")] string EventFilter,
[property: CliOption("--event-bridge-bus")] string EventBridgeBus
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}