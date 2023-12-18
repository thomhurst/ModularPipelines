using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzBillingBenefits
{
    public AzBillingBenefits(
        AzBillingBenefitsReservationOrderAliases reservationOrderAliases,
        AzBillingBenefitsSavingsPlan savingsPlan,
        AzBillingBenefitsSavingsPlanOrder savingsPlanOrder,
        AzBillingBenefitsSavingsPlanOrderAliases savingsPlanOrderAliases,
        ICommand internalCommand
    )
    {
        ReservationOrderAliases = reservationOrderAliases;
        SavingsPlan = savingsPlan;
        SavingsPlanOrder = savingsPlanOrder;
        SavingsPlanOrderAliases = savingsPlanOrderAliases;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzBillingBenefitsReservationOrderAliases ReservationOrderAliases { get; }

    public AzBillingBenefitsSavingsPlan SavingsPlan { get; }

    public AzBillingBenefitsSavingsPlanOrder SavingsPlanOrder { get; }

    public AzBillingBenefitsSavingsPlanOrderAliases SavingsPlanOrderAliases { get; }

    public async Task<CommandResult> ValidatePurchase(AzBillingBenefitsValidatePurchaseOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzBillingBenefitsValidatePurchaseOptions(), token);
    }
}

