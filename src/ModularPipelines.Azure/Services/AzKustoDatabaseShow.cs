using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("kusto", "database")]
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