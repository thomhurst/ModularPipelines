using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

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