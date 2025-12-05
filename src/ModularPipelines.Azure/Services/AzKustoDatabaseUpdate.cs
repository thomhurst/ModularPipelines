using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("kusto", "database")]
public class AzKustoDatabaseUpdate
{
    public AzKustoDatabaseUpdate(
        AzKustoDatabaseUpdateKusto kusto
    )
    {
        Kusto = kusto;
    }

    public AzKustoDatabaseUpdateKusto Kusto { get; }
}