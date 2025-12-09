using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("grafana", "update-workspace")]
public record AwsGrafanaUpdateWorkspaceOptions(
[property: CliOption("--workspace-id")] string WorkspaceId
) : AwsOptions
{
    [CliOption("--account-access-type")]
    public string? AccountAccessType { get; set; }

    [CliOption("--network-access-control")]
    public string? NetworkAccessControl { get; set; }

    [CliOption("--organization-role-name")]
    public string? OrganizationRoleName { get; set; }

    [CliOption("--permission-type")]
    public string? PermissionType { get; set; }

    [CliOption("--stack-set-name")]
    public string? StackSetName { get; set; }

    [CliOption("--vpc-configuration")]
    public string? VpcConfiguration { get; set; }

    [CliOption("--workspace-data-sources")]
    public string[]? WorkspaceDataSources { get; set; }

    [CliOption("--workspace-description")]
    public string? WorkspaceDescription { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [CliOption("--workspace-notification-destinations")]
    public string[]? WorkspaceNotificationDestinations { get; set; }

    [CliOption("--workspace-organizational-units")]
    public string[]? WorkspaceOrganizationalUnits { get; set; }

    [CliOption("--workspace-role-arn")]
    public string? WorkspaceRoleArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}