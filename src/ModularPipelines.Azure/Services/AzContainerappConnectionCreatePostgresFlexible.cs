using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "connection", "create")]
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