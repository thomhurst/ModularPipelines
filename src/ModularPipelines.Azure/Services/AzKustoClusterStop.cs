using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

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