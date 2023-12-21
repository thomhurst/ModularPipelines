using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto", "cluster")]
public class AzKustoClusterDelete
{
    public AzKustoClusterDelete(
        AzKustoClusterDeleteKusto kusto
    )
    {
        Kusto = kusto;
    }

    public AzKustoClusterDeleteKusto Kusto { get; }
}