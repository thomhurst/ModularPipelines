using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzRelay
{
    public AzRelay(
        AzRelayHyco hyco,
        AzRelayNamespace @namespace,
        AzRelayWcfrelay wcfrelay
    )
    {
        Hyco = hyco;
        Namespace = @namespace;
        Wcfrelay = wcfrelay;
    }

    public AzRelayHyco Hyco { get; }

    public AzRelayNamespace Namespace { get; }

    public AzRelayWcfrelay Wcfrelay { get; }
}