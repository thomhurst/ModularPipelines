using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datasync", "describe-storage-system-resources")]
public record AwsDatasyncDescribeStorageSystemResourcesOptions(
[property: CliOption("--discovery-job-arn")] string DiscoveryJobArn,
[property: CliOption("--resource-type")] string ResourceType
) : AwsOptions
{
    [CliOption("--resource-ids")]
    public string[]? ResourceIds { get; set; }

    [CliOption("--filter")]
    public IEnumerable<KeyValue>? Filter { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}