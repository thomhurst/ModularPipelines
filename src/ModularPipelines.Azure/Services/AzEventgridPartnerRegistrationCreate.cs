using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "partner", "registration")]
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