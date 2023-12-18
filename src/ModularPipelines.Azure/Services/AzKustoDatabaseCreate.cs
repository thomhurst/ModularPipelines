using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto", "database")]
public class AzKustoDatabaseCreate
{
    public AzKustoDatabaseCreate(
        AzKustoDatabaseCreateKusto kusto
    )
    {
        Kusto = kusto;
    }

    public AzKustoDatabaseCreateKusto Kusto { get; }
}

