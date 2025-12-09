using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "partner", "topic")]
public class AzEventgridPartnerTopicActivate
{
    public AzEventgridPartnerTopicActivate(
        AzEventgridPartnerTopicActivateEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridPartnerTopicActivateEventgrid Eventgrid { get; }
}