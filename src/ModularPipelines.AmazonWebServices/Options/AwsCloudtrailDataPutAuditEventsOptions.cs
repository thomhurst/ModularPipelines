using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudtrail-data", "put-audit-events")]
public record AwsCloudtrailDataPutAuditEventsOptions(
[property: CliOption("--audit-events")] string[] AuditEvents,
[property: CliOption("--channel-arn")] string ChannelArn
) : AwsOptions
{
    [CliOption("--external-id")]
    public string? ExternalId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}