using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzRole
{
    public AzRole(
        AzRoleAssignment assignment,
        AzRoleDefinition definition
    )
    {
        Assignment = assignment;
        Definition = definition;
    }

    public AzRoleAssignment Assignment { get; }

    public AzRoleDefinition Definition { get; }
}