using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("policy", "assignment", "non-compliance-message", "create")]
public record AzPolicyAssignmentNonComplianceMessageCreateOptions(
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