using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

