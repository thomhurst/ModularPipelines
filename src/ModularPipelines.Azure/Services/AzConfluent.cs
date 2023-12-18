using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzConfluent
{
    public AzConfluent(
        AzConfluentOfferDetail offerDetail,
        AzConfluentOrganization organization,
        AzConfluentTerms terms
    )
    {
        OfferDetail = offerDetail;
        Organization = organization;
        Terms = terms;
    }

    public AzConfluentOfferDetail OfferDetail { get; }

    public AzConfluentOrganization Organization { get; }

    public AzConfluentTerms Terms { get; }
}

