using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("grafana", "create-workspace")]
public record AwsGrafanaCreateWorkspaceOptions(
[property: CliOption("--account-access-type")] string AccountAccessType,
[property: CliOption("--authentication-providers")] string[] AuthenticationProviders,
[property: CliOption("--permission-type")] string PermissionType
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--configuration")]
    public string? Configuration { get; set; }

    [CliOption("--grafana-version")]
    public string? GrafanaVersion { get; set; }

    [CliOption("--network-access-control")]
    public string? NetworkAccessControl { get; set; }

    [CliOption("--organization-role-name")]
    public string? OrganizationRoleName { get; set; }

    [CliOption("--stack-set-name")]
    public string? StackSetName { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

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