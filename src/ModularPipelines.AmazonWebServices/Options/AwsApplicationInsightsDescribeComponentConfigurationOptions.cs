using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("application-insights", "describe-component-configuration")]
public record AwsApplicationInsightsDescribeComponentConfigurationOptions(
[property: CliOption("--resource-group-name")] string ResourceGroupName,
[property: CliOption("--component-name")] string ComponentName
) : AwsOptions
{
    [CliOption("--account-id")]
    public string? AccountId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}