using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "topic")]
public class AzEventgridTopicShow
{
    public AzEventgridTopicShow(
        AzEventgridTopicShowEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridTopicShowEventgrid Eventgrid { get; }
}