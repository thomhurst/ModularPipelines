using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid")]
public class AzEventgridPartner
{
    public AzEventgridPartner(
        AzEventgridPartnerConfiguration configuration,
        AzEventgridPartnerDestination destination,
        AzEventgridPartnerNamespace @namespace,
        AzEventgridPartnerRegistration registration,
        AzEventgridPartnerTopic topic,
        AzEventgridPartnerVerifiedPartner verifiedPartner
    )
    {
        Configuration = configuration;
        Destination = destination;
        Namespace = @namespace;
        Registration = registration;
        Topic = topic;
        VerifiedPartner = verifiedPartner;
    }

    public AzEventgridPartnerConfiguration Configuration { get; }

    public AzEventgridPartnerDestination Destination { get; }

    public AzEventgridPartnerNamespace Namespace { get; }

    public AzEventgridPartnerRegistration Registration { get; }

    public AzEventgridPartnerTopic Topic { get; }

    public AzEventgridPartnerVerifiedPartner VerifiedPartner { get; }
}