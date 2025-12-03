using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "partner", "topic")]
public class AzEventgridPartnerTopicDelete
{
    public AzEventgridPartnerTopicDelete(
        AzEventgridPartnerTopicDeleteEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridPartnerTopicDeleteEventgrid Eventgrid { get; }
}