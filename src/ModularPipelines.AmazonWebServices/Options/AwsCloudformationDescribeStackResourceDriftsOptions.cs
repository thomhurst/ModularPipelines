using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudformation", "describe-stack-resource-drifts")]
public record AwsCloudformationDescribeStackResourceDriftsOptions(
[property: CommandSwitch("--stack-name")] string StackName
) : AwsOptions
{
    [CommandSwitch("--stack-resource-drift-status-filters")]
    public string[]? StackResourceDriftStatusFilters { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}