using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "partner", "registration")]
public class AzEventgridPartnerRegistrationShow
{
    public AzEventgridPartnerRegistrationShow(
        AzEventgridPartnerRegistrationShowEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridPartnerRegistrationShowEventgrid Eventgrid { get; }
}