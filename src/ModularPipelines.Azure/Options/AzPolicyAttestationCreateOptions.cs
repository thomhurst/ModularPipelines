using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("policy", "attestation", "create")]
public record AzPolicyAttestationCreateOptions(
[property: CliOption("--attestation-name")] string AttestationName,
[property: CliOption("--policy-assignment")] string PolicyAssignment
) : AzOptions
{
    [CliOption("--assessment-date")]
    public string? AssessmentDate { get; set; }

    [CliOption("--comments")]
    public string? Comments { get; set; }

    [CliOption("--compliance-state")]
    public string? ComplianceState { get; set; }

    [CliOption("--definition-reference-id")]
    public string? DefinitionReferenceId { get; set; }

    [CliOption("--evidence")]
    public string? Evidence { get; set; }

    [CliOption("--expires-on")]
    public string? ExpiresOn { get; set; }

    [CliOption("--metadata")]
    public string? Metadata { get; set; }

    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliOption("--owner")]
    public string? Owner { get; set; }

    [CliOption("--parent")]
    public string? Parent { get; set; }

    [CliOption("--resource")]
    public string? Resource { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }
}