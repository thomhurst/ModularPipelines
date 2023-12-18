using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzPolicy
{
    public AzPolicy(
        AzPolicyAssignment assignment,
        AzPolicyAttestation attestation,
        AzPolicyDefinition definition,
        AzPolicyEvent @event,
        AzPolicyExemption exemption,
        AzPolicyMetadata metadata,
        AzPolicyRemediation remediation,
        AzPolicySetDefinition setDefinition,
        AzPolicyState state
    )
    {
        Assignment = assignment;
        Attestation = attestation;
        Definition = definition;
        Event = @event;
        Exemption = exemption;
        Metadata = metadata;
        Remediation = remediation;
        SetDefinition = setDefinition;
        State = state;
    }

    public AzPolicyAssignment Assignment { get; }

    public AzPolicyAttestation Attestation { get; }

    public AzPolicyDefinition Definition { get; }

    public AzPolicyEvent Event { get; }

    public AzPolicyExemption Exemption { get; }

    public AzPolicyMetadata Metadata { get; }

    public AzPolicyRemediation Remediation { get; }

    public AzPolicySetDefinition SetDefinition { get; }

    public AzPolicyState State { get; }
}