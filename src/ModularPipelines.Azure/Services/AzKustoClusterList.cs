using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("kusto", "cluster")]
public class AzKustoClusterList
{
    public AzKustoClusterList(
        AzKustoClusterListKusto kusto
    )
    {
        Kusto = kusto;
    }

    public AzKustoClusterListKusto Kusto { get; }
}