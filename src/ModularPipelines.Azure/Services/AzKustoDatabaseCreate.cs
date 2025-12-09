using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("kusto", "database")]
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