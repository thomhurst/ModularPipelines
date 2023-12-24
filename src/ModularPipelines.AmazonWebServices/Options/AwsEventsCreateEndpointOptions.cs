using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("events", "create-endpoint")]
public record AwsEventsCreateEndpointOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--routing-config")] string RoutingConfig,
[property: CommandSwitch("--event-buses")] string[] EventBuses
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--replication-config")]
    public string? ReplicationConfig { get; set; }

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}