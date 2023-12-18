using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzDynatrace
{
    public AzDynatrace(
        AzDynatraceMonitor monitor
    )
    {
        Monitor = monitor;
    }

    public AzDynatraceMonitor Monitor { get; }
}