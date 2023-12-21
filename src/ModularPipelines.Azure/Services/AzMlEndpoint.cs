using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

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