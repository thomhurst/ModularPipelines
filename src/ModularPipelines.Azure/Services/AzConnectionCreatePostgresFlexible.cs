using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connection", "create")]
public class AzConnectionCreatePostgresFlexible
{
    public AzConnectionCreatePostgresFlexible(
        AzConnectionCreatePostgresFlexibleServiceconnectorPasswordless serviceconnectorPasswordless
    )
    {
        ServiceconnectorPasswordless = serviceconnectorPasswordless;
    }

    public AzConnectionCreatePostgresFlexibleServiceconnectorPasswordless ServiceconnectorPasswordless { get; }
}