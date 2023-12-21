using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "auth")]
public class AzWebappAuthShow
{
    public AzWebappAuthShow(
        AzWebappAuthShowAuthV2 authV2
    )
    {
        AuthV2 = authV2;
    }

    public AzWebappAuthShowAuthV2 AuthV2 { get; }
}