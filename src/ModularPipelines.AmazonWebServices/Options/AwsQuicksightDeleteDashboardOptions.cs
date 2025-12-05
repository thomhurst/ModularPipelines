using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "delete-dashboard")]
public record AwsQuicksightDeleteDashboardOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--dashboard-id")] string DashboardId
) : AwsOptions
{
    [CliOption("--version-number")]
    public long? VersionNumber { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}