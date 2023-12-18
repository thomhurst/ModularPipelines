using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

