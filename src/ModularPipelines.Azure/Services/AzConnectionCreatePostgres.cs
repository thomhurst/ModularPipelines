using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connection", "create")]
public class AzConnectionCreatePostgres
{
    public AzConnectionCreatePostgres(
        AzConnectionCreatePostgresServiceconnectorPasswordless serviceconnectorPasswordless
    )
    {
        ServiceconnectorPasswordless = serviceconnectorPasswordless;
    }

    public AzConnectionCreatePostgresServiceconnectorPasswordless ServiceconnectorPasswordless { get; }
}