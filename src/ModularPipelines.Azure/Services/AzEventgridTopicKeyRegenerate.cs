using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "topic", "key")]
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