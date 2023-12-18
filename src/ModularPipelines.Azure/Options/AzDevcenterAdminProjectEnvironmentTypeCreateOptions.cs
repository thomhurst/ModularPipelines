using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devcenter", "admin", "project-environment-type", "create")]
public record AzDevcenterAdminProjectEnvironmentTypeCreateOptions(
[property: CommandSwitch("--deployment-target-id")] string DeploymentTargetId,
[property: CommandSwitch("--environment-type-name")] string EnvironmentTypeName,
[property: CommandSwitch("--project")] string Project,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--status")] string Status
) : AzOptions
{
    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--identity-type")]
    public string? IdentityType { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--roles")]
    public string? Roles { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--user-assigned-identities")]
    public string? UserAssignedIdentities { get; set; }

    [CommandSwitch("--user-role-assignments")]
    public string? UserRoleAssignments { get; set; }
}