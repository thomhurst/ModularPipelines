using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

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