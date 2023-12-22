using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

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