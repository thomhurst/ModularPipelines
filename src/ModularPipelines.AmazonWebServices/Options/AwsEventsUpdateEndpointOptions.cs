using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("events", "update-endpoint")]
public record AwsEventsUpdateEndpointOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--routing-config")]
    public string? RoutingConfig { get; set; }

    [CommandSwitch("--replication-config")]
    public string? ReplicationConfig { get; set; }

    [CommandSwitch("--event-buses")]
    public string[]? EventBuses { get; set; }

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}