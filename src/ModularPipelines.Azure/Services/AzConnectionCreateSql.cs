using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connection", "create")]
public class AzConnectionCreateSql
{
    public AzConnectionCreateSql(
        AzConnectionCreateSqlServiceconnectorPasswordless serviceconnectorPasswordless
    )
    {
        ServiceconnectorPasswordless = serviceconnectorPasswordless;
    }

    public AzConnectionCreateSqlServiceconnectorPasswordless ServiceconnectorPasswordless { get; }
}