using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("kusto", "cluster")]
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