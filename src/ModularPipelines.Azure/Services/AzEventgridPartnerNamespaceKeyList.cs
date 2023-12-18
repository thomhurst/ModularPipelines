using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "namespace", "key")]
public class AzEventgridPartnerNamespaceKeyList
{
    public AzEventgridPartnerNamespaceKeyList(
        AzEventgridPartnerNamespaceKeyListEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridPartnerNamespaceKeyListEventgrid Eventgrid { get; }
}