using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

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