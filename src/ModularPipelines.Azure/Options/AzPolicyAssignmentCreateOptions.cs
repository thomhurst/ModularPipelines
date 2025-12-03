using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("policy", "assignment", "create")]
public record AzPolicyAssignmentCreateOptions : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--enforcement-mode")]
    public string? EnforcementMode { get; set; }

    [CliOption("--identity-scope")]
    public string? IdentityScope { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--mi-system-assigned")]
    public string? MiSystemAssigned { get; set; }

    [CliOption("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--not-scopes")]
    public string? NotScopes { get; set; }

    [CliOption("--params")]
    public string? Params { get; set; }

    [CliOption("--policy")]
    public string? Policy { get; set; }

    [CliOption("--policy-set-definition")]
    public string? PolicySetDefinition { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--role")]
    public string? Role { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }
}