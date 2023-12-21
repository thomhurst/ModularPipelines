using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

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