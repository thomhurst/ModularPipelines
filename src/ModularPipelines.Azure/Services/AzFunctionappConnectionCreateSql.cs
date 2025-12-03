using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("functionapp", "connection", "create")]
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