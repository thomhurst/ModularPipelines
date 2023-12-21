using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto", "database")]
public class AzKustoDatabaseList
{
    public AzKustoDatabaseList(
        AzKustoDatabaseListKusto kusto
    )
    {
        Kusto = kusto;
    }

    public AzKustoDatabaseListKusto Kusto { get; }
}