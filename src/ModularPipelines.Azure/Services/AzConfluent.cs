using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

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