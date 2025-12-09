using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("webapp", "auth")]
public class AzWebappAuthUpdate
{
    public AzWebappAuthUpdate(
        AzWebappAuthUpdateAuthV2 authV2
    )
    {
        AuthV2 = authV2;
    }

    public AzWebappAuthUpdateAuthV2 AuthV2 { get; }
}