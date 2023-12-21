using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "topic", "key")]
public class AzEventgridTopicKeyList
{
    public AzEventgridTopicKeyList(
        AzEventgridTopicKeyListEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridTopicKeyListEventgrid Eventgrid { get; }
}