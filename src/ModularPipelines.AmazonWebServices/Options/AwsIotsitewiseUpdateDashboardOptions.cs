using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "update-dashboard")]
public record AwsIotsitewiseUpdateDashboardOptions(
[property: CliOption("--dashboard-id")] string DashboardId,
[property: CliOption("--dashboard-name")] string DashboardName,
[property: CliOption("--dashboard-definition")] string DashboardDefinition
) : AwsOptions
{
    [CliOption("--dashboard-description")]
    public string? DashboardDescription { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}