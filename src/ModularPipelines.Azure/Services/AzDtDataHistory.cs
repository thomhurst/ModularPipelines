using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("dt")]
public class AzDtDataHistory
{
    public AzDtDataHistory(
        AzDtDataHistoryConnection connection
    )
    {
        Connection = connection;
    }

    public AzDtDataHistoryConnection Connection { get; }
}