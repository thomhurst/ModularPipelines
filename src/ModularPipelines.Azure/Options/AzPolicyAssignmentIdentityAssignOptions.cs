using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("policy", "assignment", "identity", "assign")]
public record AzPolicyAssignmentIdentityAssignOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--identity-scope")]
    public string? IdentityScope { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--role")]
    public string? Role { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }

    [CliOption("--system-assigned")]
    public string? SystemAssigned { get; set; }

    [CliOption("--user-assigned")]
    public string? UserAssigned { get; set; }
}