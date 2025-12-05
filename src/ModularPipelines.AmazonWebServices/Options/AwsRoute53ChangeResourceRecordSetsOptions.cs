using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53", "change-resource-record-sets")]
public record AwsRoute53ChangeResourceRecordSetsOptions(
[property: CliOption("--hosted-zone-id")] string HostedZoneId,
[property: CliOption("--change-batch")] string ChangeBatch
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}