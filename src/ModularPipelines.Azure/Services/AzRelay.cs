using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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