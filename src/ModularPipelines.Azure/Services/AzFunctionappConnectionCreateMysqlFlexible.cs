using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("functionapp", "connection", "create")]
public class AzFunctionappConnectionCreateMysqlFlexible
{
    public AzFunctionappConnectionCreateMysqlFlexible(
        AzFunctionappConnectionCreateMysqlFlexibleServiceconnectorPasswordless serviceconnectorPasswordless
    )
    {
        ServiceconnectorPasswordless = serviceconnectorPasswordless;
    }

    public AzFunctionappConnectionCreateMysqlFlexibleServiceconnectorPasswordless ServiceconnectorPasswordless { get; }
}