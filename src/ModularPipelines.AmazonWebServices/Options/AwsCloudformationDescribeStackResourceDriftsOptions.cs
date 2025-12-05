using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "describe-stack-resource-drifts")]
public record AwsCloudformationDescribeStackResourceDriftsOptions(
[property: CliOption("--stack-name")] string StackName
) : AwsOptions
{
    [CliOption("--stack-resource-drift-status-filters")]
    public string[]? StackResourceDriftStatusFilters { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}