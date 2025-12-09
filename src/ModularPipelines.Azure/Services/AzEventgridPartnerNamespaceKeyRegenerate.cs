using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "partner", "namespace", "key")]
public class AzEventgridPartnerNamespaceKeyRegenerate
{
    public AzEventgridPartnerNamespaceKeyRegenerate(
        AzEventgridPartnerNamespaceKeyRegenerateEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridPartnerNamespaceKeyRegenerateEventgrid Eventgrid { get; }
}