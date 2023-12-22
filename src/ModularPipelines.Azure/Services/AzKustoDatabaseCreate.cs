using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

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