using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

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