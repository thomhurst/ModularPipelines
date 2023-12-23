using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

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

    public async Task<CommandResult> AssociateMember(AwsInspector2AssociateMemberOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetAccountStatus(AwsInspector2BatchGetAccountStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspector2BatchGetAccountStatusOptions(), token);
    }

    public async Task<CommandResult> BatchGetCodeSnippet(AwsInspector2BatchGetCodeSnippetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetFindingDetails(AwsInspector2BatchGetFindingDetailsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetFreeTrialInfo(AwsInspector2BatchGetFreeTrialInfoOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetMemberEc2DeepInspectionStatus(AwsInspector2BatchGetMemberEc2DeepInspectionStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspector2BatchGetMemberEc2DeepInspectionStatusOptions(), token);
    }

    public async Task<CommandResult> BatchUpdateMemberEc2DeepInspectionStatus(AwsInspector2BatchUpdateMemberEc2DeepInspectionStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelFindingsReport(AwsInspector2CancelFindingsReportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelSbomExport(AwsInspector2CancelSbomExportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateFilter(AwsInspector2CreateFilterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateFindingsReport(AwsInspector2CreateFindingsReportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSbomExport(AwsInspector2CreateSbomExportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteFilter(AwsInspector2DeleteFilterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeOrganizationConfiguration(AwsInspector2DescribeOrganizationConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspector2DescribeOrganizationConfigurationOptions(), token);
    }

    public async Task<CommandResult> Disable(AwsInspector2DisableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspector2DisableOptions(), token);
    }

    public async Task<CommandResult> DisableDelegatedAdminAccount(AwsInspector2DisableDelegatedAdminAccountOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateMember(AwsInspector2DisassociateMemberOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Enable(AwsInspector2EnableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableDelegatedAdminAccount(AwsInspector2EnableDelegatedAdminAccountOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetConfiguration(AwsInspector2GetConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspector2GetConfigurationOptions(), token);
    }

    public async Task<CommandResult> GetDelegatedAdminAccount(AwsInspector2GetDelegatedAdminAccountOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspector2GetDelegatedAdminAccountOptions(), token);
    }

    public async Task<CommandResult> GetEc2DeepInspectionConfiguration(AwsInspector2GetEc2DeepInspectionConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspector2GetEc2DeepInspectionConfigurationOptions(), token);
    }

    public async Task<CommandResult> GetEncryptionKey(AwsInspector2GetEncryptionKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetFindingsReportStatus(AwsInspector2GetFindingsReportStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspector2GetFindingsReportStatusOptions(), token);
    }

    public async Task<CommandResult> GetMember(AwsInspector2GetMemberOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSbomExport(AwsInspector2GetSbomExportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAccountPermissions(AwsInspector2ListAccountPermissionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspector2ListAccountPermissionsOptions(), token);
    }

    public async Task<CommandResult> ListCoverage(AwsInspector2ListCoverageOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspector2ListCoverageOptions(), token);
    }

    public async Task<CommandResult> ListCoverageStatistics(AwsInspector2ListCoverageStatisticsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspector2ListCoverageStatisticsOptions(), token);
    }

    public async Task<CommandResult> ListDelegatedAdminAccounts(AwsInspector2ListDelegatedAdminAccountsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspector2ListDelegatedAdminAccountsOptions(), token);
    }

    public async Task<CommandResult> ListFilters(AwsInspector2ListFiltersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspector2ListFiltersOptions(), token);
    }

    public async Task<CommandResult> ListFindingAggregations(AwsInspector2ListFindingAggregationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListFindings(AwsInspector2ListFindingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspector2ListFindingsOptions(), token);
    }

    public async Task<CommandResult> ListMembers(AwsInspector2ListMembersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspector2ListMembersOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsInspector2ListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListUsageTotals(AwsInspector2ListUsageTotalsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspector2ListUsageTotalsOptions(), token);
    }

    public async Task<CommandResult> ResetEncryptionKey(AwsInspector2ResetEncryptionKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SearchVulnerabilities(AwsInspector2SearchVulnerabilitiesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsInspector2TagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsInspector2UntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateConfiguration(AwsInspector2UpdateConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateEc2DeepInspectionConfiguration(AwsInspector2UpdateEc2DeepInspectionConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsInspector2UpdateEc2DeepInspectionConfigurationOptions(), token);
    }

    public async Task<CommandResult> UpdateEncryptionKey(AwsInspector2UpdateEncryptionKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateFilter(AwsInspector2UpdateFilterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateOrgEc2DeepInspectionConfiguration(AwsInspector2UpdateOrgEc2DeepInspectionConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateOrganizationConfiguration(AwsInspector2UpdateOrganizationConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}