using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "topic")]
public class AzEventgridPartnerTopicDeactivate
{
    public AzEventgridPartnerTopicDeactivate(
        AzEventgridPartnerTopicDeactivateEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridPartnerTopicDeactivateEventgrid Eventgrid { get; }
}

