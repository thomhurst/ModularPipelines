using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt")]
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