using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("policy", "assignment", "list")]
public record AzPolicyAssignmentListOptions : AzOptions
{
    [CliFlag("--disable-scope-strict-match")]
    public bool? DisableScopeStrictMatch { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }
}