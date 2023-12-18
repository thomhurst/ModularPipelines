using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("policy", "assignment", "non-compliance-message", "delete")]
public record AzPolicyAssignmentNonComplianceMessageDeleteOptions(
[property: CommandSwitch("--message")] string Message,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--policy-definition-reference-id")]
    public string? PolicyDefinitionReferenceId { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }
}