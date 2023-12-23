using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsBillingconductor
{
    public AwsBillingconductor(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AssociateAccounts(AwsBillingconductorAssociateAccountsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociatePricingRules(AwsBillingconductorAssociatePricingRulesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchAssociateResourcesToCustomLineItem(AwsBillingconductorBatchAssociateResourcesToCustomLineItemOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchDisassociateResourcesFromCustomLineItem(AwsBillingconductorBatchDisassociateResourcesFromCustomLineItemOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateBillingGroup(AwsBillingconductorCreateBillingGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCustomLineItem(AwsBillingconductorCreateCustomLineItemOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePricingPlan(AwsBillingconductorCreatePricingPlanOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePricingRule(AwsBillingconductorCreatePricingRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteBillingGroup(AwsBillingconductorDeleteBillingGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCustomLineItem(AwsBillingconductorDeleteCustomLineItemOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePricingPlan(AwsBillingconductorDeletePricingPlanOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePricingRule(AwsBillingconductorDeletePricingRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateAccounts(AwsBillingconductorDisassociateAccountsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociatePricingRules(AwsBillingconductorDisassociatePricingRulesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetBillingGroupCostReport(AwsBillingconductorGetBillingGroupCostReportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAccountAssociations(AwsBillingconductorListAccountAssociationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsBillingconductorListAccountAssociationsOptions(), token);
    }

    public async Task<CommandResult> ListBillingGroupCostReports(AwsBillingconductorListBillingGroupCostReportsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsBillingconductorListBillingGroupCostReportsOptions(), token);
    }

    public async Task<CommandResult> ListBillingGroups(AwsBillingconductorListBillingGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsBillingconductorListBillingGroupsOptions(), token);
    }

    public async Task<CommandResult> ListCustomLineItemVersions(AwsBillingconductorListCustomLineItemVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListCustomLineItems(AwsBillingconductorListCustomLineItemsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsBillingconductorListCustomLineItemsOptions(), token);
    }

    public async Task<CommandResult> ListPricingPlans(AwsBillingconductorListPricingPlansOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsBillingconductorListPricingPlansOptions(), token);
    }

    public async Task<CommandResult> ListPricingPlansAssociatedWithPricingRule(AwsBillingconductorListPricingPlansAssociatedWithPricingRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListPricingRules(AwsBillingconductorListPricingRulesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsBillingconductorListPricingRulesOptions(), token);
    }

    public async Task<CommandResult> ListPricingRulesAssociatedToPricingPlan(AwsBillingconductorListPricingRulesAssociatedToPricingPlanOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListResourcesAssociatedToCustomLineItem(AwsBillingconductorListResourcesAssociatedToCustomLineItemOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsBillingconductorListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsBillingconductorTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsBillingconductorUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateBillingGroup(AwsBillingconductorUpdateBillingGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateCustomLineItem(AwsBillingconductorUpdateCustomLineItemOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdatePricingPlan(AwsBillingconductorUpdatePricingPlanOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdatePricingRule(AwsBillingconductorUpdatePricingRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}