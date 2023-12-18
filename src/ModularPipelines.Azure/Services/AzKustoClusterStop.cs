using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto", "cluster")]
public class AzKustoClusterStop
{
    public AzKustoClusterStop(
        AzKustoClusterStopKusto kusto
    )
    {
        Kusto = kusto;
    }

    public AzKustoClusterStopKusto Kusto { get; }
}