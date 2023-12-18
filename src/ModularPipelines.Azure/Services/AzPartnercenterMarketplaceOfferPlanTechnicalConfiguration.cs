using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer", "plan")]
public class AzPartnercenterMarketplaceOfferPlanTechnicalConfiguration
{
    public AzPartnercenterMarketplaceOfferPlanTechnicalConfiguration(
        AzPartnercenterMarketplaceOfferPlanTechnicalConfigurationPackage package,
        ICommand internalCommand
    )
    {
        Package = package;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzPartnercenterMarketplaceOfferPlanTechnicalConfigurationPackage Package { get; }

    public async Task<CommandResult> Show(AzPartnercenterMarketplaceOfferPlanTechnicalConfigurationShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}