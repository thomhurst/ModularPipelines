using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "partner", "registration")]
public class AzEventgridPartnerRegistrationList
{
    public AzEventgridPartnerRegistrationList(
        AzEventgridPartnerRegistrationListEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridPartnerRegistrationListEventgrid Eventgrid { get; }
}