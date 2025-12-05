using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "update-dashboard-permissions")]
public record AwsQuicksightUpdateDashboardPermissionsOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--dashboard-id")] string DashboardId
) : AwsOptions
{
    [CliOption("--grant-permissions")]
    public string[]? GrantPermissions { get; set; }

    [CliOption("--revoke-permissions")]
    public string[]? RevokePermissions { get; set; }

    [CliOption("--grant-link-permissions")]
    public string[]? GrantLinkPermissions { get; set; }

    [CliOption("--revoke-link-permissions")]
    public string[]? RevokeLinkPermissions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}