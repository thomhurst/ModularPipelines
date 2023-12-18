using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer", "plan", "technical-configuration")]
public class AzPartnercenterMarketplaceOfferPlanTechnicalConfigurationPackage
{
    public AzPartnercenterMarketplaceOfferPlanTechnicalConfigurationPackage(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Add(AzPartnercenterMarketplaceOfferPlanTechnicalConfigurationPackageAddOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzPartnercenterMarketplaceOfferPlanTechnicalConfigurationPackageDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}