using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

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