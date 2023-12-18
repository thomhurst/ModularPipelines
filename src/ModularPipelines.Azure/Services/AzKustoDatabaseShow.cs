using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto", "database")]
public class AzKustoDatabaseShow
{
    public AzKustoDatabaseShow(
        AzKustoDatabaseShowKusto kusto
    )
    {
        Kusto = kusto;
    }

    public AzKustoDatabaseShowKusto Kusto { get; }
}