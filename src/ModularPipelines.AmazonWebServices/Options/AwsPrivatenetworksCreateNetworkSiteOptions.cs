using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("privatenetworks", "create-network-site")]
public record AwsPrivatenetworksCreateNetworkSiteOptions(
[property: CliOption("--network-arn")] string NetworkArn,
[property: CliOption("--network-site-name")] string NetworkSiteName
) : AwsOptions
{
    [CliOption("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CliOption("--availability-zone-id")]
    public string? AvailabilityZoneId { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--pending-plan")]
    public string? PendingPlan { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}