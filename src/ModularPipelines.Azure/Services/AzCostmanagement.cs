using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzCostmanagement
{
    public AzCostmanagement(
        AzCostmanagementExport export
    )
    {
        Export = export;
    }

    public AzCostmanagementExport Export { get; }
}

