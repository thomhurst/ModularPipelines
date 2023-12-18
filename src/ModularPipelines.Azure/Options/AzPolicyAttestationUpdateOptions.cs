using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("policy", "attestation", "update")]
public record AzPolicyAttestationUpdateOptions(
[property: CommandSwitch("--attestation-name")] string AttestationName
) : AzOptions
{
    [CommandSwitch("--assessment-date")]
    public string? AssessmentDate { get; set; }

    [CommandSwitch("--comments")]
    public string? Comments { get; set; }

    [CommandSwitch("--compliance-state")]
    public string? ComplianceState { get; set; }

    [CommandSwitch("--definition-reference-id")]
    public string? DefinitionReferenceId { get; set; }

    [CommandSwitch("--evidence")]
    public string? Evidence { get; set; }

    [CommandSwitch("--expires-on")]
    public string? ExpiresOn { get; set; }

    [CommandSwitch("--metadata")]
    public string? Metadata { get; set; }

    [CommandSwitch("--namespace")]
    public string? Namespace { get; set; }

    [CommandSwitch("--owner")]
    public string? Owner { get; set; }

    [CommandSwitch("--parent")]
    public string? Parent { get; set; }

    [CommandSwitch("--policy-assignment")]
    public string? PolicyAssignment { get; set; }

    [CommandSwitch("--resource")]
    public string? Resource { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }
}