using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("events", "create-endpoint")]
public record AwsEventsCreateEndpointOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--routing-config")] string RoutingConfig,
[property: CliOption("--event-buses")] string[] EventBuses
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--replication-config")]
    public string? ReplicationConfig { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}