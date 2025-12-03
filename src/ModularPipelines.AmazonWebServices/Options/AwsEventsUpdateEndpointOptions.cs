using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("events", "update-endpoint")]
public record AwsEventsUpdateEndpointOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--routing-config")]
    public string? RoutingConfig { get; set; }

    [CliOption("--replication-config")]
    public string? ReplicationConfig { get; set; }

    [CliOption("--event-buses")]
    public string[]? EventBuses { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}