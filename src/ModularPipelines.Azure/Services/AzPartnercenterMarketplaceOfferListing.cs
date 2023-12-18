using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer")]
public class AzPartnercenterMarketplaceOfferListing
{
    public AzPartnercenterMarketplaceOfferListing(
        AzPartnercenterMarketplaceOfferListingContact contact,
        AzPartnercenterMarketplaceOfferListingMedia media,
        AzPartnercenterMarketplaceOfferListingUri uri,
        ICommand internalCommand
    )
    {
        Contact = contact;
        Media = media;
        Uri = uri;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzPartnercenterMarketplaceOfferListingContact Contact { get; }

    public AzPartnercenterMarketplaceOfferListingMedia Media { get; }

    public AzPartnercenterMarketplaceOfferListingUri Uri { get; }

    public async Task<CommandResult> Show(AzPartnercenterMarketplaceOfferListingShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzPartnercenterMarketplaceOfferListingUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}