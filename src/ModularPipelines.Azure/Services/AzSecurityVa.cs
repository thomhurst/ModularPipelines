using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

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