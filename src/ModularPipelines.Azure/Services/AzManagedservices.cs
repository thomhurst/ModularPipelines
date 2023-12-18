using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzManagedservices
{
    public AzManagedservices(
        AzManagedservicesAssignment assignment,
        AzManagedservicesDefinition definition
    )
    {
        Assignment = assignment;
        Definition = definition;
    }

    public AzManagedservicesAssignment Assignment { get; }

    public AzManagedservicesDefinition Definition { get; }
}

