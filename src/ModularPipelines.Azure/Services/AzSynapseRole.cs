using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse")]
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