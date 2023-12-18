using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer")]
public class AzPartnercenterMarketplaceOfferPlan
{
    public AzPartnercenterMarketplaceOfferPlan(
        AzPartnercenterMarketplaceOfferPlanListing listing,
        AzPartnercenterMarketplaceOfferPlanTechnicalConfiguration technicalConfiguration,
        ICommand internalCommand
    )
    {
        Listing = listing;
        TechnicalConfiguration = technicalConfiguration;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzPartnercenterMarketplaceOfferPlanListing Listing { get; }

    public AzPartnercenterMarketplaceOfferPlanTechnicalConfiguration TechnicalConfiguration { get; }

    public async Task<CommandResult> Create(AzPartnercenterMarketplaceOfferPlanCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzPartnercenterMarketplaceOfferPlanDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzPartnercenterMarketplaceOfferPlanListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzPartnercenterMarketplaceOfferPlanShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}