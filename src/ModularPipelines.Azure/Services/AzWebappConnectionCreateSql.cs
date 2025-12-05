using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("webapp", "connection", "create")]
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