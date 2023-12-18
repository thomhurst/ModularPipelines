using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "registration")]
public class AzEventgridPartnerRegistrationCreate
{
    public AzEventgridPartnerRegistrationCreate(
        AzEventgridPartnerRegistrationCreateEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridPartnerRegistrationCreateEventgrid Eventgrid { get; }
}

