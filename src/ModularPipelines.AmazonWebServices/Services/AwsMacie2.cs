using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsMacie2
{
    public AwsMacie2(
        AwsMacie2Wait wait,
        ICommand internalCommand
    )
    {
        Wait = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AwsMacie2Wait Wait { get; }

    public async Task<CommandResult> AcceptInvitation(AwsMacie2AcceptInvitationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetCustomDataIdentifiers(AwsMacie2BatchGetCustomDataIdentifiersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2BatchGetCustomDataIdentifiersOptions(), token);
    }

    public async Task<CommandResult> CreateAllowList(AwsMacie2CreateAllowListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateClassificationJob(AwsMacie2CreateClassificationJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCustomDataIdentifier(AwsMacie2CreateCustomDataIdentifierOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateFindingsFilter(AwsMacie2CreateFindingsFilterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateInvitations(AwsMacie2CreateInvitationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateMember(AwsMacie2CreateMemberOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSampleFindings(AwsMacie2CreateSampleFindingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2CreateSampleFindingsOptions(), token);
    }

    public async Task<CommandResult> DeclineInvitations(AwsMacie2DeclineInvitationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAllowList(AwsMacie2DeleteAllowListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCustomDataIdentifier(AwsMacie2DeleteCustomDataIdentifierOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteFindingsFilter(AwsMacie2DeleteFindingsFilterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteInvitations(AwsMacie2DeleteInvitationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteMember(AwsMacie2DeleteMemberOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeBuckets(AwsMacie2DescribeBucketsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2DescribeBucketsOptions(), token);
    }

    public async Task<CommandResult> DescribeClassificationJob(AwsMacie2DescribeClassificationJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeOrganizationConfiguration(AwsMacie2DescribeOrganizationConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2DescribeOrganizationConfigurationOptions(), token);
    }

    public async Task<CommandResult> DisableMacie(AwsMacie2DisableMacieOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2DisableMacieOptions(), token);
    }

    public async Task<CommandResult> DisableOrganizationAdminAccount(AwsMacie2DisableOrganizationAdminAccountOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateFromAdministratorAccount(AwsMacie2DisassociateFromAdministratorAccountOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2DisassociateFromAdministratorAccountOptions(), token);
    }

    public async Task<CommandResult> DisassociateFromMasterAccount(AwsMacie2DisassociateFromMasterAccountOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2DisassociateFromMasterAccountOptions(), token);
    }

    public async Task<CommandResult> DisassociateMember(AwsMacie2DisassociateMemberOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableMacie(AwsMacie2EnableMacieOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2EnableMacieOptions(), token);
    }

    public async Task<CommandResult> EnableOrganizationAdminAccount(AwsMacie2EnableOrganizationAdminAccountOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAdministratorAccount(AwsMacie2GetAdministratorAccountOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2GetAdministratorAccountOptions(), token);
    }

    public async Task<CommandResult> GetAllowList(AwsMacie2GetAllowListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAutomatedDiscoveryConfiguration(AwsMacie2GetAutomatedDiscoveryConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2GetAutomatedDiscoveryConfigurationOptions(), token);
    }

    public async Task<CommandResult> GetBucketStatistics(AwsMacie2GetBucketStatisticsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2GetBucketStatisticsOptions(), token);
    }

    public async Task<CommandResult> GetClassificationExportConfiguration(AwsMacie2GetClassificationExportConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2GetClassificationExportConfigurationOptions(), token);
    }

    public async Task<CommandResult> GetClassificationScope(AwsMacie2GetClassificationScopeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCustomDataIdentifier(AwsMacie2GetCustomDataIdentifierOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetFindingStatistics(AwsMacie2GetFindingStatisticsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetFindings(AwsMacie2GetFindingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetFindingsFilter(AwsMacie2GetFindingsFilterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetFindingsPublicationConfiguration(AwsMacie2GetFindingsPublicationConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2GetFindingsPublicationConfigurationOptions(), token);
    }

    public async Task<CommandResult> GetInvitationsCount(AwsMacie2GetInvitationsCountOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2GetInvitationsCountOptions(), token);
    }

    public async Task<CommandResult> GetMacieSession(AwsMacie2GetMacieSessionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2GetMacieSessionOptions(), token);
    }

    public async Task<CommandResult> GetMasterAccount(AwsMacie2GetMasterAccountOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2GetMasterAccountOptions(), token);
    }

    public async Task<CommandResult> GetMember(AwsMacie2GetMemberOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetResourceProfile(AwsMacie2GetResourceProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRevealConfiguration(AwsMacie2GetRevealConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2GetRevealConfigurationOptions(), token);
    }

    public async Task<CommandResult> GetSensitiveDataOccurrences(AwsMacie2GetSensitiveDataOccurrencesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSensitiveDataOccurrencesAvailability(AwsMacie2GetSensitiveDataOccurrencesAvailabilityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSensitivityInspectionTemplate(AwsMacie2GetSensitivityInspectionTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetUsageStatistics(AwsMacie2GetUsageStatisticsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2GetUsageStatisticsOptions(), token);
    }

    public async Task<CommandResult> GetUsageTotals(AwsMacie2GetUsageTotalsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2GetUsageTotalsOptions(), token);
    }

    public async Task<CommandResult> ListAllowLists(AwsMacie2ListAllowListsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2ListAllowListsOptions(), token);
    }

    public async Task<CommandResult> ListClassificationJobs(AwsMacie2ListClassificationJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2ListClassificationJobsOptions(), token);
    }

    public async Task<CommandResult> ListClassificationScopes(AwsMacie2ListClassificationScopesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2ListClassificationScopesOptions(), token);
    }

    public async Task<CommandResult> ListCustomDataIdentifiers(AwsMacie2ListCustomDataIdentifiersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2ListCustomDataIdentifiersOptions(), token);
    }

    public async Task<CommandResult> ListFindings(AwsMacie2ListFindingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2ListFindingsOptions(), token);
    }

    public async Task<CommandResult> ListFindingsFilters(AwsMacie2ListFindingsFiltersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2ListFindingsFiltersOptions(), token);
    }

    public async Task<CommandResult> ListInvitations(AwsMacie2ListInvitationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2ListInvitationsOptions(), token);
    }

    public async Task<CommandResult> ListManagedDataIdentifiers(AwsMacie2ListManagedDataIdentifiersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2ListManagedDataIdentifiersOptions(), token);
    }

    public async Task<CommandResult> ListMembers(AwsMacie2ListMembersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2ListMembersOptions(), token);
    }

    public async Task<CommandResult> ListOrganizationAdminAccounts(AwsMacie2ListOrganizationAdminAccountsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2ListOrganizationAdminAccountsOptions(), token);
    }

    public async Task<CommandResult> ListResourceProfileArtifacts(AwsMacie2ListResourceProfileArtifactsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListResourceProfileDetections(AwsMacie2ListResourceProfileDetectionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSensitivityInspectionTemplates(AwsMacie2ListSensitivityInspectionTemplatesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2ListSensitivityInspectionTemplatesOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsMacie2ListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutClassificationExportConfiguration(AwsMacie2PutClassificationExportConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutFindingsPublicationConfiguration(AwsMacie2PutFindingsPublicationConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2PutFindingsPublicationConfigurationOptions(), token);
    }

    public async Task<CommandResult> SearchResources(AwsMacie2SearchResourcesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2SearchResourcesOptions(), token);
    }

    public async Task<CommandResult> TagResource(AwsMacie2TagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TestCustomDataIdentifier(AwsMacie2TestCustomDataIdentifierOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsMacie2UntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAllowList(AwsMacie2UpdateAllowListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAutomatedDiscoveryConfiguration(AwsMacie2UpdateAutomatedDiscoveryConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateClassificationJob(AwsMacie2UpdateClassificationJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateClassificationScope(AwsMacie2UpdateClassificationScopeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateFindingsFilter(AwsMacie2UpdateFindingsFilterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateMacieSession(AwsMacie2UpdateMacieSessionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2UpdateMacieSessionOptions(), token);
    }

    public async Task<CommandResult> UpdateMemberSession(AwsMacie2UpdateMemberSessionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateOrganizationConfiguration(AwsMacie2UpdateOrganizationConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2UpdateOrganizationConfigurationOptions(), token);
    }

    public async Task<CommandResult> UpdateResourceProfile(AwsMacie2UpdateResourceProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateResourceProfileDetections(AwsMacie2UpdateResourceProfileDetectionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateRevealConfiguration(AwsMacie2UpdateRevealConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSensitivityInspectionTemplate(AwsMacie2UpdateSensitivityInspectionTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}