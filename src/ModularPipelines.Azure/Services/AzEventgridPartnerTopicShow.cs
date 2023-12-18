using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "topic")]
public class AzEventgridPartnerTopicShow
{
    public AzEventgridPartnerTopicShow(
        AzEventgridPartnerTopicShowEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridPartnerTopicShowEventgrid Eventgrid { get; }
}

