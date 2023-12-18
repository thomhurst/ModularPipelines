using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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