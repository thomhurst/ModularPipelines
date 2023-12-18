using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

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