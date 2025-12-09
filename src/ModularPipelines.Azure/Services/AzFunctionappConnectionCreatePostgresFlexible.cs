using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("functionapp", "connection", "create")]
public class AzFunctionappConnectionCreatePostgresFlexible
{
    public AzFunctionappConnectionCreatePostgresFlexible(
        AzFunctionappConnectionCreatePostgresFlexibleServiceconnectorPasswordless serviceconnectorPasswordless
    )
    {
        ServiceconnectorPasswordless = serviceconnectorPasswordless;
    }

    public AzFunctionappConnectionCreatePostgresFlexibleServiceconnectorPasswordless ServiceconnectorPasswordless { get; }
}