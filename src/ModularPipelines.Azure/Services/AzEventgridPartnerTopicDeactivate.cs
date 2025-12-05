using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "partner", "topic")]
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