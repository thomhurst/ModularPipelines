using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "namespace", "key")]
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