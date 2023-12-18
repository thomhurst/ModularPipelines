using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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