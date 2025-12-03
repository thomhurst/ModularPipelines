using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("blueprint", "assignment", "create")]
public record AzBlueprintAssignmentCreateOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--blueprint-version")]
    public string? BlueprintVersion { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--identity-type")]
    public string? IdentityType { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--locks-excluded-principals")]
    public string? LocksExcludedPrincipals { get; set; }

    [CliOption("--locks-mode")]
    public string? LocksMode { get; set; }

    [CliOption("--management-group")]
    public string? ManagementGroup { get; set; }

    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--resource-group-value")]
    public string? ResourceGroupValue { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--user-assigned-identity")]
    public string? UserAssignedIdentity { get; set; }
}