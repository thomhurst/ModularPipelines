using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "extension-topic")]
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