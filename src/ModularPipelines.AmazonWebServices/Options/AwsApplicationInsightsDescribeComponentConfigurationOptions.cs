using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("application-insights", "describe-component-configuration")]
public record AwsApplicationInsightsDescribeComponentConfigurationOptions(
[property: CommandSwitch("--resource-group-name")] string ResourceGroupName,
[property: CommandSwitch("--component-name")] string ComponentName
) : AwsOptions
{
    [CommandSwitch("--account-id")]
    public string? AccountId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}