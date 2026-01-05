using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    public async Task<CommandResult> AcceptAdministratorInvitation(AwsSecurityhubAcceptAdministratorInvitationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> BatchDeleteAutomationRules(AwsSecurityhubBatchDeleteAutomationRulesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> BatchDisableStandards(AwsSecurityhubBatchDisableStandardsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> BatchEnableStandards(AwsSecurityhubBatchEnableStandardsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> BatchGetAutomationRules(AwsSecurityhubBatchGetAutomationRulesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> BatchGetConfigurationPolicyAssociations(AwsSecurityhubBatchGetConfigurationPolicyAssociationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> BatchGetSecurityControls(AwsSecurityhubBatchGetSecurityControlsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> BatchGetStandardsControlAssociations(AwsSecurityhubBatchGetStandardsControlAssociationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> BatchImportFindings(AwsSecurityhubBatchImportFindingsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> BatchUpdateAutomationRules(AwsSecurityhubBatchUpdateAutomationRulesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> BatchUpdateFindings(AwsSecurityhubBatchUpdateFindingsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> BatchUpdateStandardsControlAssociations(AwsSecurityhubBatchUpdateStandardsControlAssociationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateActionTarget(AwsSecurityhubCreateActionTargetOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateAutomationRule(AwsSecurityhubCreateAutomationRuleOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateConfigurationPolicy(AwsSecurityhubCreateConfigurationPolicyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateFindingAggregator(AwsSecurityhubCreateFindingAggregatorOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateInsight(AwsSecurityhubCreateInsightOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateMembers(AwsSecurityhubCreateMembersOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeclineInvitations(AwsSecurityhubDeclineInvitationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteActionTarget(AwsSecurityhubDeleteActionTargetOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteConfigurationPolicy(AwsSecurityhubDeleteConfigurationPolicyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteFindingAggregator(AwsSecurityhubDeleteFindingAggregatorOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteInsight(AwsSecurityhubDeleteInsightOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteInvitations(AwsSecurityhubDeleteInvitationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteMembers(AwsSecurityhubDeleteMembersOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeActionTargets(AwsSecurityhubDescribeActionTargetsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubDescribeActionTargetsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeHub(AwsSecurityhubDescribeHubOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubDescribeHubOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeOrganizationConfiguration(AwsSecurityhubDescribeOrganizationConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubDescribeOrganizationConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeProducts(AwsSecurityhubDescribeProductsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubDescribeProductsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeStandards(AwsSecurityhubDescribeStandardsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubDescribeStandardsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeStandardsControls(AwsSecurityhubDescribeStandardsControlsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisableImportFindingsForProduct(AwsSecurityhubDisableImportFindingsForProductOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisableOrganizationAdminAccount(AwsSecurityhubDisableOrganizationAdminAccountOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisableSecurityHub(AwsSecurityhubDisableSecurityHubOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubDisableSecurityHubOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateFromAdministratorAccount(AwsSecurityhubDisassociateFromAdministratorAccountOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubDisassociateFromAdministratorAccountOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateMembers(AwsSecurityhubDisassociateMembersOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> EnableImportFindingsForProduct(AwsSecurityhubEnableImportFindingsForProductOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> EnableOrganizationAdminAccount(AwsSecurityhubEnableOrganizationAdminAccountOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> EnableSecurityHub(AwsSecurityhubEnableSecurityHubOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubEnableSecurityHubOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetAdministratorAccount(AwsSecurityhubGetAdministratorAccountOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubGetAdministratorAccountOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetConfigurationPolicy(AwsSecurityhubGetConfigurationPolicyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetConfigurationPolicyAssociation(AwsSecurityhubGetConfigurationPolicyAssociationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetEnabledStandards(AwsSecurityhubGetEnabledStandardsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubGetEnabledStandardsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetFindingAggregator(AwsSecurityhubGetFindingAggregatorOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetFindingHistory(AwsSecurityhubGetFindingHistoryOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetFindings(AwsSecurityhubGetFindingsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubGetFindingsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetInsightResults(AwsSecurityhubGetInsightResultsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetInsights(AwsSecurityhubGetInsightsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubGetInsightsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetInvitationsCount(AwsSecurityhubGetInvitationsCountOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubGetInvitationsCountOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetMembers(AwsSecurityhubGetMembersOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetSecurityControlDefinition(AwsSecurityhubGetSecurityControlDefinitionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> InviteMembers(AwsSecurityhubInviteMembersOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListAutomationRules(AwsSecurityhubListAutomationRulesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubListAutomationRulesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListConfigurationPolicies(AwsSecurityhubListConfigurationPoliciesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubListConfigurationPoliciesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListConfigurationPolicyAssociations(AwsSecurityhubListConfigurationPolicyAssociationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubListConfigurationPolicyAssociationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListEnabledProductsForImport(AwsSecurityhubListEnabledProductsForImportOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubListEnabledProductsForImportOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListFindingAggregators(AwsSecurityhubListFindingAggregatorsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubListFindingAggregatorsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListInvitations(AwsSecurityhubListInvitationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubListInvitationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListMembers(AwsSecurityhubListMembersOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubListMembersOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListOrganizationAdminAccounts(AwsSecurityhubListOrganizationAdminAccountsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubListOrganizationAdminAccountsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListSecurityControlDefinitions(AwsSecurityhubListSecurityControlDefinitionsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubListSecurityControlDefinitionsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListStandardsControlAssociations(AwsSecurityhubListStandardsControlAssociationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListTagsForResource(AwsSecurityhubListTagsForResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartConfigurationPolicyAssociation(AwsSecurityhubStartConfigurationPolicyAssociationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartConfigurationPolicyDisassociation(AwsSecurityhubStartConfigurationPolicyDisassociationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> TagResource(AwsSecurityhubTagResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UntagResource(AwsSecurityhubUntagResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateActionTarget(AwsSecurityhubUpdateActionTargetOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateConfigurationPolicy(AwsSecurityhubUpdateConfigurationPolicyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateFindingAggregator(AwsSecurityhubUpdateFindingAggregatorOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateFindings(AwsSecurityhubUpdateFindingsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateInsight(AwsSecurityhubUpdateInsightOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateOrganizationConfiguration(AwsSecurityhubUpdateOrganizationConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubUpdateOrganizationConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateSecurityControl(AwsSecurityhubUpdateSecurityControlOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateSecurityHubConfiguration(AwsSecurityhubUpdateSecurityHubConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecurityhubUpdateSecurityHubConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateStandardsControl(AwsSecurityhubUpdateStandardsControlOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}