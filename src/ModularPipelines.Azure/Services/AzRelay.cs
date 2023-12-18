using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzRelay
{
    public AzRelay(
        AzRelayHyco hyco,
        AzRelayNamespace @Namespace,
        AzRelayWcfrelay wcfrelay
    )
    {
        Hyco = hyco;
        Namespace = @Namespace;
        Wcfrelay = wcfrelay;
    }

    public AzRelayHyco Hyco { get; }

    public AzRelayNamespace Namespace { get; }

    public AzRelayWcfrelay Wcfrelay { get; }
}

