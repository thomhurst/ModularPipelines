using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto", "database")]
public class AzKustoDatabaseDelete
{
    public AzKustoDatabaseDelete(
        AzKustoDatabaseDeleteKusto kusto
    )
    {
        Kusto = kusto;
    }

    public AzKustoDatabaseDeleteKusto Kusto { get; }
}

