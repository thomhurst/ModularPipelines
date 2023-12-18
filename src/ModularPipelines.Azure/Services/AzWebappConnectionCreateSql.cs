using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "connection", "create")]
public class AzWebappConnectionCreateSql
{
    public AzWebappConnectionCreateSql(
        AzWebappConnectionCreateSqlServiceconnectorPasswordless serviceconnectorPasswordless
    )
    {
        ServiceconnectorPasswordless = serviceconnectorPasswordless;
    }

    public AzWebappConnectionCreateSqlServiceconnectorPasswordless ServiceconnectorPasswordless { get; }
}

