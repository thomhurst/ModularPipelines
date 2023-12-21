using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault")]
public class AzKeyvaultRole
{
    public AzKeyvaultRole(
        AzKeyvaultRoleAssignment assignment,
        AzKeyvaultRoleDefinition definition
    )
    {
        Assignment = assignment;
        Definition = definition;
    }

    public AzKeyvaultRoleAssignment Assignment { get; }

    public AzKeyvaultRoleDefinition Definition { get; }
}