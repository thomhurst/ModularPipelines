using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("policy", "assignment", "update")]
public record AzPolicyAssignmentUpdateOptions : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--enforcement-mode")]
    public string? EnforcementMode { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--not-scopes")]
    public string? NotScopes { get; set; }

    [CliOption("--params")]
    public string? Params { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }
}