using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml")]
public class AzMlEndpoint
{
    public AzMlEndpoint(
        AzMlEndpointRealtime realtime
    )
    {
        Realtime = realtime;
    }

    public AzMlEndpointRealtime Realtime { get; }
}

