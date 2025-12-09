using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault")]
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