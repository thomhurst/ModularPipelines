using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("containerapp", "connection", "create")]
public class AzContainerappConnectionCreateSql
{
    public AzContainerappConnectionCreateSql(
        AzContainerappConnectionCreateSqlServiceconnectorPasswordless serviceconnectorPasswordless
    )
    {
        ServiceconnectorPasswordless = serviceconnectorPasswordless;
    }

    public AzContainerappConnectionCreateSqlServiceconnectorPasswordless ServiceconnectorPasswordless { get; }
}