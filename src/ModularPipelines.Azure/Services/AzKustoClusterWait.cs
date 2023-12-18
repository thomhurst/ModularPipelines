using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto", "cluster")]
public class AzKustoClusterWait
{
    public AzKustoClusterWait(
        AzKustoClusterWaitKusto kusto
    )
    {
        Kusto = kusto;
    }

    public AzKustoClusterWaitKusto Kusto { get; }
}

