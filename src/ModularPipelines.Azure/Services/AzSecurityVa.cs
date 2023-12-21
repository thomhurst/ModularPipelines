using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security")]
public class AzSecurityVa
{
    public AzSecurityVa(
        AzSecurityVaSql sql
    )
    {
        Sql = sql;
    }

    public AzSecurityVaSql Sql { get; }
}