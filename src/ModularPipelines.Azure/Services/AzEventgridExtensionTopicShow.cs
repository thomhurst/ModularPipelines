using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

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