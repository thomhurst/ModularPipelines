using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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