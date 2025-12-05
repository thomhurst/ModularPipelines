using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devcenter", "admin", "project-environment-type", "create")]
public record AzDevcenterAdminProjectEnvironmentTypeCreateOptions(
[property: CliOption("--deployment-target-id")] string DeploymentTargetId,
[property: CliOption("--environment-type-name")] string EnvironmentTypeName,
[property: CliOption("--project")] string Project,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--status")] string Status
) : AzOptions
{
    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--identity-type")]
    public string? IdentityType { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--roles")]
    public string? Roles { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--user-assigned-identities")]
    public string? UserAssignedIdentities { get; set; }

    [CliOption("--user-role-assignments")]
    public string? UserRoleAssignments { get; set; }
}