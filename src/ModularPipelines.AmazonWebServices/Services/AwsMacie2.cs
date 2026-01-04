using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    public async Task<CommandResult> AcceptInvitation(AwsMacie2AcceptInvitationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> BatchGetCustomDataIdentifiers(AwsMacie2BatchGetCustomDataIdentifiersOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2BatchGetCustomDataIdentifiersOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateAllowList(AwsMacie2CreateAllowListOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateClassificationJob(AwsMacie2CreateClassificationJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateCustomDataIdentifier(AwsMacie2CreateCustomDataIdentifierOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateFindingsFilter(AwsMacie2CreateFindingsFilterOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateInvitations(AwsMacie2CreateInvitationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateMember(AwsMacie2CreateMemberOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateSampleFindings(AwsMacie2CreateSampleFindingsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2CreateSampleFindingsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeclineInvitations(AwsMacie2DeclineInvitationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteAllowList(AwsMacie2DeleteAllowListOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteCustomDataIdentifier(AwsMacie2DeleteCustomDataIdentifierOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteFindingsFilter(AwsMacie2DeleteFindingsFilterOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteInvitations(AwsMacie2DeleteInvitationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteMember(AwsMacie2DeleteMemberOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeBuckets(AwsMacie2DescribeBucketsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2DescribeBucketsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeClassificationJob(AwsMacie2DescribeClassificationJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeOrganizationConfiguration(AwsMacie2DescribeOrganizationConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2DescribeOrganizationConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisableMacie(AwsMacie2DisableMacieOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2DisableMacieOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisableOrganizationAdminAccount(AwsMacie2DisableOrganizationAdminAccountOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateFromAdministratorAccount(AwsMacie2DisassociateFromAdministratorAccountOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2DisassociateFromAdministratorAccountOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateFromMasterAccount(AwsMacie2DisassociateFromMasterAccountOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2DisassociateFromMasterAccountOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateMember(AwsMacie2DisassociateMemberOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> EnableMacie(AwsMacie2EnableMacieOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2EnableMacieOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> EnableOrganizationAdminAccount(AwsMacie2EnableOrganizationAdminAccountOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetAdministratorAccount(AwsMacie2GetAdministratorAccountOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2GetAdministratorAccountOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetAllowList(AwsMacie2GetAllowListOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetAutomatedDiscoveryConfiguration(AwsMacie2GetAutomatedDiscoveryConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2GetAutomatedDiscoveryConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetBucketStatistics(AwsMacie2GetBucketStatisticsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2GetBucketStatisticsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetClassificationExportConfiguration(AwsMacie2GetClassificationExportConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2GetClassificationExportConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetClassificationScope(AwsMacie2GetClassificationScopeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetCustomDataIdentifier(AwsMacie2GetCustomDataIdentifierOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetFindingStatistics(AwsMacie2GetFindingStatisticsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetFindings(AwsMacie2GetFindingsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetFindingsFilter(AwsMacie2GetFindingsFilterOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetFindingsPublicationConfiguration(AwsMacie2GetFindingsPublicationConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2GetFindingsPublicationConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetInvitationsCount(AwsMacie2GetInvitationsCountOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2GetInvitationsCountOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetMacieSession(AwsMacie2GetMacieSessionOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2GetMacieSessionOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetMasterAccount(AwsMacie2GetMasterAccountOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2GetMasterAccountOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetMember(AwsMacie2GetMemberOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetResourceProfile(AwsMacie2GetResourceProfileOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetRevealConfiguration(AwsMacie2GetRevealConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2GetRevealConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetSensitiveDataOccurrences(AwsMacie2GetSensitiveDataOccurrencesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetSensitiveDataOccurrencesAvailability(AwsMacie2GetSensitiveDataOccurrencesAvailabilityOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetSensitivityInspectionTemplate(AwsMacie2GetSensitivityInspectionTemplateOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetUsageStatistics(AwsMacie2GetUsageStatisticsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2GetUsageStatisticsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetUsageTotals(AwsMacie2GetUsageTotalsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2GetUsageTotalsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListAllowLists(AwsMacie2ListAllowListsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2ListAllowListsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListClassificationJobs(AwsMacie2ListClassificationJobsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2ListClassificationJobsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListClassificationScopes(AwsMacie2ListClassificationScopesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2ListClassificationScopesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListCustomDataIdentifiers(AwsMacie2ListCustomDataIdentifiersOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2ListCustomDataIdentifiersOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListFindings(AwsMacie2ListFindingsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2ListFindingsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListFindingsFilters(AwsMacie2ListFindingsFiltersOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2ListFindingsFiltersOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListInvitations(AwsMacie2ListInvitationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2ListInvitationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListManagedDataIdentifiers(AwsMacie2ListManagedDataIdentifiersOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2ListManagedDataIdentifiersOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListMembers(AwsMacie2ListMembersOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2ListMembersOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListOrganizationAdminAccounts(AwsMacie2ListOrganizationAdminAccountsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2ListOrganizationAdminAccountsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListResourceProfileArtifacts(AwsMacie2ListResourceProfileArtifactsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListResourceProfileDetections(AwsMacie2ListResourceProfileDetectionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListSensitivityInspectionTemplates(AwsMacie2ListSensitivityInspectionTemplatesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2ListSensitivityInspectionTemplatesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListTagsForResource(AwsMacie2ListTagsForResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutClassificationExportConfiguration(AwsMacie2PutClassificationExportConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutFindingsPublicationConfiguration(AwsMacie2PutFindingsPublicationConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2PutFindingsPublicationConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SearchResources(AwsMacie2SearchResourcesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2SearchResourcesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> TagResource(AwsMacie2TagResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> TestCustomDataIdentifier(AwsMacie2TestCustomDataIdentifierOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UntagResource(AwsMacie2UntagResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateAllowList(AwsMacie2UpdateAllowListOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateAutomatedDiscoveryConfiguration(AwsMacie2UpdateAutomatedDiscoveryConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateClassificationJob(AwsMacie2UpdateClassificationJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateClassificationScope(AwsMacie2UpdateClassificationScopeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateFindingsFilter(AwsMacie2UpdateFindingsFilterOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateMacieSession(AwsMacie2UpdateMacieSessionOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2UpdateMacieSessionOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateMemberSession(AwsMacie2UpdateMemberSessionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateOrganizationConfiguration(AwsMacie2UpdateOrganizationConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMacie2UpdateOrganizationConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateResourceProfile(AwsMacie2UpdateResourceProfileOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateResourceProfileDetections(AwsMacie2UpdateResourceProfileDetectionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateRevealConfiguration(AwsMacie2UpdateRevealConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateSensitivityInspectionTemplate(AwsMacie2UpdateSensitivityInspectionTemplateOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}