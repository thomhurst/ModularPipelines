using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "partner", "registration")]
public class AzEventgridPartnerRegistrationDelete
{
    public AzEventgridPartnerRegistrationDelete(
        AzEventgridPartnerRegistrationDeleteEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridPartnerRegistrationDeleteEventgrid Eventgrid { get; }
}