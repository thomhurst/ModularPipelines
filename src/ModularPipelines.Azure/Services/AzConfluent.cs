using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzConfluent
{
    public AzConfluent(
        AzConfluentOfferDetail offerDetail,
        AzConfluentOrganization organization
    )
    {
        OfferDetail = offerDetail;
        Organization = organization;
    }

    public AzConfluentOfferDetail OfferDetail { get; }

    public AzConfluentOrganization Organization { get; }
}