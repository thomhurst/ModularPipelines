using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datasync", "describe-storage-system-resources")]
public record AwsDatasyncDescribeStorageSystemResourcesOptions(
[property: CommandSwitch("--discovery-job-arn")] string DiscoveryJobArn,
[property: CommandSwitch("--resource-type")] string ResourceType
) : AwsOptions
{
    [CommandSwitch("--resource-ids")]
    public string[]? ResourceIds { get; set; }

    [CommandSwitch("--filter")]
    public IEnumerable<KeyValue>? Filter { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}