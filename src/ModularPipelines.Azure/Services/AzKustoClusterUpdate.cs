using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto", "cluster")]
public class AzKustoClusterUpdate
{
    public AzKustoClusterUpdate(
        AzKustoClusterUpdateKusto kusto
    )
    {
        Kusto = kusto;
    }

    public AzKustoClusterUpdateKusto Kusto { get; }
}