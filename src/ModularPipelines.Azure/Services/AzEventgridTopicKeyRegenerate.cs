using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "topic", "key")]
public class AzEventgridTopicKeyRegenerate
{
    public AzEventgridTopicKeyRegenerate(
        AzEventgridTopicKeyRegenerateEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridTopicKeyRegenerateEventgrid Eventgrid { get; }
}

