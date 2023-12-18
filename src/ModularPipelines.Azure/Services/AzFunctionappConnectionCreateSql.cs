using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp", "connection", "create")]
public class AzFunctionappConnectionCreateSql
{
    public AzFunctionappConnectionCreateSql(
        AzFunctionappConnectionCreateSqlServiceconnectorPasswordless serviceconnectorPasswordless
    )
    {
        ServiceconnectorPasswordless = serviceconnectorPasswordless;
    }

    public AzFunctionappConnectionCreateSqlServiceconnectorPasswordless ServiceconnectorPasswordless { get; }
}

