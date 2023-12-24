using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudwatch", "put-dashboard")]
public record AwsCloudwatchPutDashboardOptions(
[property: CommandSwitch("--dashboard-name")] string DashboardName,
[property: CommandSwitch("--dashboard-body")] string DashboardBody
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}