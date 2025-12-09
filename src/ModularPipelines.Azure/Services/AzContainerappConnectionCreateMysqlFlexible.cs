using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("containerapp", "connection", "create")]
public class AzContainerappConnectionCreateMysqlFlexible
{
    public AzContainerappConnectionCreateMysqlFlexible(
        AzContainerappConnectionCreateMysqlFlexibleServiceconnectorPasswordless serviceconnectorPasswordless
    )
    {
        ServiceconnectorPasswordless = serviceconnectorPasswordless;
    }

    public AzContainerappConnectionCreateMysqlFlexibleServiceconnectorPasswordless ServiceconnectorPasswordless { get; }
}