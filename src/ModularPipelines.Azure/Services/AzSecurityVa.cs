using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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