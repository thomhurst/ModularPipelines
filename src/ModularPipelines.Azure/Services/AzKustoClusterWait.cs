using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("kusto", "cluster")]
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