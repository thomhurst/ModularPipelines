using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto", "cluster")]
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

