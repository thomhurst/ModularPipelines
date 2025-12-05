using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("kusto", "cluster")]
public class AzKustoClusterCreate
{
    public AzKustoClusterCreate(
        AzKustoClusterCreateKusto kusto
    )
    {
        Kusto = kusto;
    }

    public AzKustoClusterCreateKusto Kusto { get; }
}