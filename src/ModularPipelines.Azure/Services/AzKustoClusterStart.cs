using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto", "cluster")]
public class AzKustoClusterStart
{
    public AzKustoClusterStart(
        AzKustoClusterStartKusto kusto
    )
    {
        Kusto = kusto;
    }

    public AzKustoClusterStartKusto Kusto { get; }
}