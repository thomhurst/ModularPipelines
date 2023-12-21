using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto", "cluster")]
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