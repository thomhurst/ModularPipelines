using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53", "list-resource-record-sets")]
public record AwsRoute53ListResourceRecordSetsOptions(
[property: CliOption("--hosted-zone-id")] string HostedZoneId
) : AwsOptions
{
    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public string? PageSize { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}