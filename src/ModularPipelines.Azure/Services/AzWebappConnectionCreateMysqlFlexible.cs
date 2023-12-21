using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "connection", "create")]
public class AzWebappConnectionCreateMysqlFlexible
{
    public AzWebappConnectionCreateMysqlFlexible(
        AzWebappConnectionCreateMysqlFlexibleServiceconnectorPasswordless serviceconnectorPasswordless
    )
    {
        ServiceconnectorPasswordless = serviceconnectorPasswordless;
    }

    public AzWebappConnectionCreateMysqlFlexibleServiceconnectorPasswordless ServiceconnectorPasswordless { get; }
}