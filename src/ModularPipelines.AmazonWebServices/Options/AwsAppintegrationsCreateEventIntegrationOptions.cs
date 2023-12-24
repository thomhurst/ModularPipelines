using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appintegrations", "create-event-integration")]
public record AwsAppintegrationsCreateEventIntegrationOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--event-filter")] string EventFilter,
[property: CommandSwitch("--event-bridge-bus")] string EventBridgeBus
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}