using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto", "database")]
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