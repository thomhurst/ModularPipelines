using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("grafana", "update-workspace")]
public record AwsGrafanaUpdateWorkspaceOptions(
[property: CommandSwitch("--workspace-id")] string WorkspaceId
) : AwsOptions
{
    [CommandSwitch("--account-access-type")]
    public string? AccountAccessType { get; set; }

    [CommandSwitch("--network-access-control")]
    public string? NetworkAccessControl { get; set; }

    [CommandSwitch("--organization-role-name")]
    public string? OrganizationRoleName { get; set; }

    [CommandSwitch("--permission-type")]
    public string? PermissionType { get; set; }

    [CommandSwitch("--stack-set-name")]
    public string? StackSetName { get; set; }

    [CommandSwitch("--vpc-configuration")]
    public string? VpcConfiguration { get; set; }

    [CommandSwitch("--workspace-data-sources")]
    public string[]? WorkspaceDataSources { get; set; }

    [CommandSwitch("--workspace-description")]
    public string? WorkspaceDescription { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [CommandSwitch("--workspace-notification-destinations")]
    public string[]? WorkspaceNotificationDestinations { get; set; }

    [CommandSwitch("--workspace-organizational-units")]
    public string[]? WorkspaceOrganizationalUnits { get; set; }

    [CommandSwitch("--workspace-role-arn")]
    public string? WorkspaceRoleArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}