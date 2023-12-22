using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace")]
public class AzPartnercenterMarketplaceOffer
{
    public AzPartnercenterMarketplaceOffer(
        AzPartnercenterMarketplaceOfferListing listing,
        AzPartnercenterMarketplaceOfferPackage package,
        AzPartnercenterMarketplaceOfferPlan plan,
        AzPartnercenterMarketplaceOfferSetup setup,
        AzPartnercenterMarketplaceOfferSubmission submission,
        ICommand internalCommand
    )
    {
        Listing = listing;
        Package = package;
        Plan = plan;
        Setup = setup;
        Submission = submission;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzPartnercenterMarketplaceOfferListing Listing { get; }

    public AzPartnercenterMarketplaceOfferPackage Package { get; }

    public AzPartnercenterMarketplaceOfferPlan Plan { get; }

    public AzPartnercenterMarketplaceOfferSetup Setup { get; }

    public AzPartnercenterMarketplaceOfferSubmission Submission { get; }

    public async Task<CommandResult> Create(AzPartnercenterMarketplaceOfferCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzPartnercenterMarketplaceOfferDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzPartnercenterMarketplaceOfferListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPartnercenterMarketplaceOfferListOptions(), token);
    }

    public async Task<CommandResult> Publish(AzPartnercenterMarketplaceOfferPublishOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzPartnercenterMarketplaceOfferShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}