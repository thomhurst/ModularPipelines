using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("ml")]
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