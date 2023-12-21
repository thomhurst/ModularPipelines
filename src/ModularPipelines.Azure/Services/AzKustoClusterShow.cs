using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto", "cluster")]
public class AzKustoClusterShow
{
    public AzKustoClusterShow(
        AzKustoClusterShowKusto kusto
    )
    {
        Kusto = kusto;
    }

    public AzKustoClusterShowKusto Kusto { get; }
}