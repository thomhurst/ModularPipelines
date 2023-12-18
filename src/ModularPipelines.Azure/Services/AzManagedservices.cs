using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

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