using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "partner", "topic")]
public class AzEventgridPartnerTopicList
{
    public AzEventgridPartnerTopicList(
        AzEventgridPartnerTopicListEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridPartnerTopicListEventgrid Eventgrid { get; }
}