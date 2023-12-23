using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsSecurityhub
{
    public AwsSecurityhub(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AcceptAdministratorInvitation(AwsSecurityhubAcceptAdministratorInvitationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchDeleteAutomationRules(AwsSecurityhubBatchDeleteAutomationRulesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchDisableStandards(AwsSecurityhubBatchDisableStandardsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchEnableStandards(AwsSecurityhubBatchEnableStandardsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetAutomationRules(AwsSecurityhubBatchGetAutomationRulesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetConfigurationPolicyAssociations(AwsSecurityhubBatchGetConfigurationPolicyAssociationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetSecurityControls(AwsSecurityhubBatchGetSecurityControlsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetStandardsControlAssociations(AwsSecurityhubBatchGetStandardsControlAssociationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchImportFindings(AwsSecurityhubBatchImportFindingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchUpdateAutomationRules(AwsSecurityhubBatchUpdateAutomationRulesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchUpdateFindings(AwsSecurityhubBatchUpdateFindingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchUpdateStandardsControlAssociations(AwsSecurityhubBatchUpdateStandardsControlAssociationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateActionTarget(AwsSecurityhubCreateActionTargetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAutomationRule(AwsSecurityhubCreateAutomationRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateConfigurationPolicy(AwsSecurityhubCreateConfigurationPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateFindingAggregator(AwsSecurityhubCreateFindingAggregatorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateInsight(AwsSecurityhubCreateInsightOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateMembers(AwsSecurityhubCreateMembersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeclineInvitations(AwsSecurityhubDeclineInvitationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteActionTarget(AwsSecurityhubDeleteActionTargetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteConfigurationPolicy(AwsSecurityhubDeleteConfigurationPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteFindingAggregator(AwsSecurityhubDeleteFindingAggregatorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteInsight(AwsSecurityhubDeleteInsightOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteInvitations(AwsSecurityhubDeleteInvitationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteMembers(AwsSecurityhubDeleteMembersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeActionTargets(AwsSecurityhubDescribeActionTargetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubDescribeActionTargetsOptions(), token);
    }

    public async Task<CommandResult> DescribeHub(AwsSecurityhubDescribeHubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubDescribeHubOptions(), token);
    }

    public async Task<CommandResult> DescribeOrganizationConfiguration(AwsSecurityhubDescribeOrganizationConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubDescribeOrganizationConfigurationOptions(), token);
    }

    public async Task<CommandResult> DescribeProducts(AwsSecurityhubDescribeProductsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubDescribeProductsOptions(), token);
    }

    public async Task<CommandResult> DescribeStandards(AwsSecurityhubDescribeStandardsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubDescribeStandardsOptions(), token);
    }

    public async Task<CommandResult> DescribeStandardsControls(AwsSecurityhubDescribeStandardsControlsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisableImportFindingsForProduct(AwsSecurityhubDisableImportFindingsForProductOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisableOrganizationAdminAccount(AwsSecurityhubDisableOrganizationAdminAccountOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisableSecurityHub(AwsSecurityhubDisableSecurityHubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubDisableSecurityHubOptions(), token);
    }

    public async Task<CommandResult> DisassociateFromAdministratorAccount(AwsSecurityhubDisassociateFromAdministratorAccountOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubDisassociateFromAdministratorAccountOptions(), token);
    }

    public async Task<CommandResult> DisassociateMembers(AwsSecurityhubDisassociateMembersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableImportFindingsForProduct(AwsSecurityhubEnableImportFindingsForProductOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableOrganizationAdminAccount(AwsSecurityhubEnableOrganizationAdminAccountOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableSecurityHub(AwsSecurityhubEnableSecurityHubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubEnableSecurityHubOptions(), token);
    }

    public async Task<CommandResult> GetAdministratorAccount(AwsSecurityhubGetAdministratorAccountOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubGetAdministratorAccountOptions(), token);
    }

    public async Task<CommandResult> GetConfigurationPolicy(AwsSecurityhubGetConfigurationPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetConfigurationPolicyAssociation(AwsSecurityhubGetConfigurationPolicyAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetEnabledStandards(AwsSecurityhubGetEnabledStandardsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubGetEnabledStandardsOptions(), token);
    }

    public async Task<CommandResult> GetFindingAggregator(AwsSecurityhubGetFindingAggregatorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetFindingHistory(AwsSecurityhubGetFindingHistoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetFindings(AwsSecurityhubGetFindingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubGetFindingsOptions(), token);
    }

    public async Task<CommandResult> GetInsightResults(AwsSecurityhubGetInsightResultsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetInsights(AwsSecurityhubGetInsightsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubGetInsightsOptions(), token);
    }

    public async Task<CommandResult> GetInvitationsCount(AwsSecurityhubGetInvitationsCountOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubGetInvitationsCountOptions(), token);
    }

    public async Task<CommandResult> GetMembers(AwsSecurityhubGetMembersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSecurityControlDefinition(AwsSecurityhubGetSecurityControlDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> InviteMembers(AwsSecurityhubInviteMembersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAutomationRules(AwsSecurityhubListAutomationRulesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubListAutomationRulesOptions(), token);
    }

    public async Task<CommandResult> ListConfigurationPolicies(AwsSecurityhubListConfigurationPoliciesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubListConfigurationPoliciesOptions(), token);
    }

    public async Task<CommandResult> ListConfigurationPolicyAssociations(AwsSecurityhubListConfigurationPolicyAssociationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubListConfigurationPolicyAssociationsOptions(), token);
    }

    public async Task<CommandResult> ListEnabledProductsForImport(AwsSecurityhubListEnabledProductsForImportOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubListEnabledProductsForImportOptions(), token);
    }

    public async Task<CommandResult> ListFindingAggregators(AwsSecurityhubListFindingAggregatorsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubListFindingAggregatorsOptions(), token);
    }

    public async Task<CommandResult> ListInvitations(AwsSecurityhubListInvitationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubListInvitationsOptions(), token);
    }

    public async Task<CommandResult> ListMembers(AwsSecurityhubListMembersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubListMembersOptions(), token);
    }

    public async Task<CommandResult> ListOrganizationAdminAccounts(AwsSecurityhubListOrganizationAdminAccountsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubListOrganizationAdminAccountsOptions(), token);
    }

    public async Task<CommandResult> ListSecurityControlDefinitions(AwsSecurityhubListSecurityControlDefinitionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubListSecurityControlDefinitionsOptions(), token);
    }

    public async Task<CommandResult> ListStandardsControlAssociations(AwsSecurityhubListStandardsControlAssociationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsSecurityhubListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartConfigurationPolicyAssociation(AwsSecurityhubStartConfigurationPolicyAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartConfigurationPolicyDisassociation(AwsSecurityhubStartConfigurationPolicyDisassociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsSecurityhubTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsSecurityhubUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateActionTarget(AwsSecurityhubUpdateActionTargetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateConfigurationPolicy(AwsSecurityhubUpdateConfigurationPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateFindingAggregator(AwsSecurityhubUpdateFindingAggregatorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateFindings(AwsSecurityhubUpdateFindingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateInsight(AwsSecurityhubUpdateInsightOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateOrganizationConfiguration(AwsSecurityhubUpdateOrganizationConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubUpdateOrganizationConfigurationOptions(), token);
    }

    public async Task<CommandResult> UpdateSecurityControl(AwsSecurityhubUpdateSecurityControlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSecurityHubConfiguration(AwsSecurityhubUpdateSecurityHubConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubUpdateSecurityHubConfigurationOptions(), token);
    }

    public async Task<CommandResult> UpdateStandardsControl(AwsSecurityhubUpdateStandardsControlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}