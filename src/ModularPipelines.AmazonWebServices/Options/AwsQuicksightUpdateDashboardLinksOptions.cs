using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "update-dashboard-links")]
public record AwsQuicksightUpdateDashboardLinksOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--dashboard-id")] string DashboardId,
[property: CliOption("--link-entities")] string[] LinkEntities
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}