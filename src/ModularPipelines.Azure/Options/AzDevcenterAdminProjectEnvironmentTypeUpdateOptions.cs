using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devcenter", "admin", "project-environment-type", "update")]
public record AzDevcenterAdminProjectEnvironmentTypeUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--deployment-target-id")]
    public string? DeploymentTargetId { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--environment-type-name")]
    public string? EnvironmentTypeName { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--identity-type")]
    public string? IdentityType { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--roles")]
    public string? Roles { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--user-assigned-identities")]
    public string? UserAssignedIdentities { get; set; }

    [CliOption("--user-role-assignments")]
    public string? UserRoleAssignments { get; set; }
}