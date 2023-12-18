using System.Diagnostics.CodeAnalysis;

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