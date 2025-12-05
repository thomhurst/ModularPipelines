using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("webapp", "connection", "create")]
public class AzWebappConnectionCreatePostgres
{
    public AzWebappConnectionCreatePostgres(
        AzWebappConnectionCreatePostgresServiceconnectorPasswordless serviceconnectorPasswordless
    )
    {
        ServiceconnectorPasswordless = serviceconnectorPasswordless;
    }

    public AzWebappConnectionCreatePostgresServiceconnectorPasswordless ServiceconnectorPasswordless { get; }
}