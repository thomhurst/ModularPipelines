using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto", "database")]
public class AzKustoDatabaseWait
{
    public AzKustoDatabaseWait(
        AzKustoDatabaseWaitKusto kusto
    )
    {
        Kusto = kusto;
    }

    public AzKustoDatabaseWaitKusto Kusto { get; }
}