using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto", "database")]
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