using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("grafana", "create-workspace")]
public record AwsGrafanaCreateWorkspaceOptions(
[property: CommandSwitch("--account-access-type")] string AccountAccessType,
[property: CommandSwitch("--authentication-providers")] string[] AuthenticationProviders,
[property: CommandSwitch("--permission-type")] string PermissionType
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--configuration")]
    public string? Configuration { get; set; }

    [CommandSwitch("--grafana-version")]
    public string? GrafanaVersion { get; set; }

    [CommandSwitch("--network-access-control")]
    public string? NetworkAccessControl { get; set; }

    [CommandSwitch("--organization-role-name")]
    public string? OrganizationRoleName { get; set; }

    [CommandSwitch("--stack-set-name")]
    public string? StackSetName { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

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