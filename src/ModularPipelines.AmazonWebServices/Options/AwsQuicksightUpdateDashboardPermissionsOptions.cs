using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "update-dashboard-permissions")]
public record AwsQuicksightUpdateDashboardPermissionsOptions(
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--dashboard-id")] string DashboardId
) : AwsOptions
{
    [CommandSwitch("--grant-permissions")]
    public string[]? GrantPermissions { get; set; }

    [CommandSwitch("--revoke-permissions")]
    public string[]? RevokePermissions { get; set; }

    [CommandSwitch("--grant-link-permissions")]
    public string[]? GrantLinkPermissions { get; set; }

    [CommandSwitch("--revoke-link-permissions")]
    public string[]? RevokeLinkPermissions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}