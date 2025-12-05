using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("synapse")]
public class AzSynapseRole
{
    public AzSynapseRole(
        AzSynapseRoleAssignment assignment,
        AzSynapseRoleDefinition definition,
        AzSynapseRoleScope scope
    )
    {
        Assignment = assignment;
        Definition = definition;
        Scope = scope;
    }

    public AzSynapseRoleAssignment Assignment { get; }

    public AzSynapseRoleDefinition Definition { get; }

    public AzSynapseRoleScope Scope { get; }
}