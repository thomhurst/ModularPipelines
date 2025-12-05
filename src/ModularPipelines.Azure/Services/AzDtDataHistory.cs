using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("dt")]
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