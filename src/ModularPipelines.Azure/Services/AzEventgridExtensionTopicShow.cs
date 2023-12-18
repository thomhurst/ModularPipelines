using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "extension-topic")]
public class AzEventgridExtensionTopicShow
{
    public AzEventgridExtensionTopicShow(
        AzEventgridExtensionTopicShowEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridExtensionTopicShowEventgrid Eventgrid { get; }
}