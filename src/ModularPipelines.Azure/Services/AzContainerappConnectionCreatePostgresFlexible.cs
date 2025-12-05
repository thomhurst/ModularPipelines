using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("containerapp", "connection", "create")]
public class AzContainerappConnectionCreatePostgresFlexible
{
    public AzContainerappConnectionCreatePostgresFlexible(
        AzContainerappConnectionCreatePostgresFlexibleServiceconnectorPasswordless serviceconnectorPasswordless
    )
    {
        ServiceconnectorPasswordless = serviceconnectorPasswordless;
    }

    public AzContainerappConnectionCreatePostgresFlexibleServiceconnectorPasswordless ServiceconnectorPasswordless { get; }
}