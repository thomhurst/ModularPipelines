using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing-benefits")]
public class AzBillingBenefitsSavingsPlanOrder
{
    public AzBillingBenefitsSavingsPlanOrder(
        AzBillingBenefitsSavingsPlanOrderSavingsPlan savingsPlan,
        ICommand internalCommand
    )
    {
        SavingsPlan = savingsPlan;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzBillingBenefitsSavingsPlanOrderSavingsPlan SavingsPlan { get; }

    public async Task<CommandResult> Elevate(AzBillingBenefitsSavingsPlanOrderElevateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzBillingBenefitsSavingsPlanOrderListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzBillingBenefitsSavingsPlanOrderShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}