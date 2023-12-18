using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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