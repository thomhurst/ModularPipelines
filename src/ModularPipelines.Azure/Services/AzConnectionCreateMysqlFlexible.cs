using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("connection", "create")]
public class AzConnectionCreateMysqlFlexible
{
    public AzConnectionCreateMysqlFlexible(
        AzConnectionCreateMysqlFlexibleServiceconnectorPasswordless serviceconnectorPasswordless
    )
    {
        ServiceconnectorPasswordless = serviceconnectorPasswordless;
    }

    public AzConnectionCreateMysqlFlexibleServiceconnectorPasswordless ServiceconnectorPasswordless { get; }
}