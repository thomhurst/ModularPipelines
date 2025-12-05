using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudwatch", "put-dashboard")]
public record AwsCloudwatchPutDashboardOptions(
[property: CliOption("--dashboard-name")] string DashboardName,
[property: CliOption("--dashboard-body")] string DashboardBody
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}