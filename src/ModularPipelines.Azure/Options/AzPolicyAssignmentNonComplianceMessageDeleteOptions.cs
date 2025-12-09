using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("policy", "assignment", "non-compliance-message", "delete")]
public record AzPolicyAssignmentNonComplianceMessageDeleteOptions(
[property: CliOption("--message")] string Message,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--policy-definition-reference-id")]
    public string? PolicyDefinitionReferenceId { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }
}