using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsInspector2
{
    public AwsInspector2(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AssociateMember(AwsInspector2AssociateMemberOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> BatchGetAccountStatus(AwsInspector2BatchGetAccountStatusOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspector2BatchGetAccountStatusOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> BatchGetCodeSnippet(AwsInspector2BatchGetCodeSnippetOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> BatchGetFindingDetails(AwsInspector2BatchGetFindingDetailsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> BatchGetFreeTrialInfo(AwsInspector2BatchGetFreeTrialInfoOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> BatchGetMemberEc2DeepInspectionStatus(AwsInspector2BatchGetMemberEc2DeepInspectionStatusOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspector2BatchGetMemberEc2DeepInspectionStatusOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> BatchUpdateMemberEc2DeepInspectionStatus(AwsInspector2BatchUpdateMemberEc2DeepInspectionStatusOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CancelFindingsReport(AwsInspector2CancelFindingsReportOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CancelSbomExport(AwsInspector2CancelSbomExportOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateFilter(AwsInspector2CreateFilterOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateFindingsReport(AwsInspector2CreateFindingsReportOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateSbomExport(AwsInspector2CreateSbomExportOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteFilter(AwsInspector2DeleteFilterOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeOrganizationConfiguration(AwsInspector2DescribeOrganizationConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspector2DescribeOrganizationConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> Disable(AwsInspector2DisableOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspector2DisableOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisableDelegatedAdminAccount(AwsInspector2DisableDelegatedAdminAccountOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateMember(AwsInspector2DisassociateMemberOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> Enable(AwsInspector2EnableOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> EnableDelegatedAdminAccount(AwsInspector2EnableDelegatedAdminAccountOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetConfiguration(AwsInspector2GetConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspector2GetConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetDelegatedAdminAccount(AwsInspector2GetDelegatedAdminAccountOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspector2GetDelegatedAdminAccountOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetEc2DeepInspectionConfiguration(AwsInspector2GetEc2DeepInspectionConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspector2GetEc2DeepInspectionConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetEncryptionKey(AwsInspector2GetEncryptionKeyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetFindingsReportStatus(AwsInspector2GetFindingsReportStatusOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspector2GetFindingsReportStatusOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetMember(AwsInspector2GetMemberOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetSbomExport(AwsInspector2GetSbomExportOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListAccountPermissions(AwsInspector2ListAccountPermissionsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspector2ListAccountPermissionsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListCoverage(AwsInspector2ListCoverageOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspector2ListCoverageOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListCoverageStatistics(AwsInspector2ListCoverageStatisticsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspector2ListCoverageStatisticsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListDelegatedAdminAccounts(AwsInspector2ListDelegatedAdminAccountsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspector2ListDelegatedAdminAccountsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListFilters(AwsInspector2ListFiltersOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspector2ListFiltersOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListFindingAggregations(AwsInspector2ListFindingAggregationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListFindings(AwsInspector2ListFindingsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspector2ListFindingsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListMembers(AwsInspector2ListMembersOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspector2ListMembersOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListTagsForResource(AwsInspector2ListTagsForResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListUsageTotals(AwsInspector2ListUsageTotalsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspector2ListUsageTotalsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ResetEncryptionKey(AwsInspector2ResetEncryptionKeyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SearchVulnerabilities(AwsInspector2SearchVulnerabilitiesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> TagResource(AwsInspector2TagResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UntagResource(AwsInspector2UntagResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateConfiguration(AwsInspector2UpdateConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateEc2DeepInspectionConfiguration(AwsInspector2UpdateEc2DeepInspectionConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspector2UpdateEc2DeepInspectionConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateEncryptionKey(AwsInspector2UpdateEncryptionKeyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateFilter(AwsInspector2UpdateFilterOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateOrgEc2DeepInspectionConfiguration(AwsInspector2UpdateOrgEc2DeepInspectionConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateOrganizationConfiguration(AwsInspector2UpdateOrganizationConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}