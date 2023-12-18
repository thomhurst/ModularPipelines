using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("policy", "assignment", "create")]
public record AzPolicyAssignmentCreateOptions : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--enforcement-mode")]
    public string? EnforcementMode { get; set; }

    [CommandSwitch("--identity-scope")]
    public string? IdentityScope { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--mi-system-assigned")]
    public string? MiSystemAssigned { get; set; }

    [CommandSwitch("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--not-scopes")]
    public string? NotScopes { get; set; }

    [CommandSwitch("--params")]
    public string? Params { get; set; }

    [CommandSwitch("--policy")]
    public string? Policy { get; set; }

    [CommandSwitch("--policy-set-definition")]
    public string? PolicySetDefinition { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--role")]
    public string? Role { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }
}