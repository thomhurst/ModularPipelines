using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("containerapp", "connection", "create")]
public class AzContainerappConnectionCreatePostgres
{
    public AzContainerappConnectionCreatePostgres(
        AzContainerappConnectionCreatePostgresServiceconnectorPasswordless serviceconnectorPasswordless
    )
    {
        ServiceconnectorPasswordless = serviceconnectorPasswordless;
    }

    public AzContainerappConnectionCreatePostgresServiceconnectorPasswordless ServiceconnectorPasswordless { get; }
}