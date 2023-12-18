using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("policy", "assignment", "list")]
public record AzPolicyAssignmentListOptions : AzOptions
{
    [BooleanCommandSwitch("--disable-scope-strict-match")]
    public bool? DisableScopeStrictMatch { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }
}