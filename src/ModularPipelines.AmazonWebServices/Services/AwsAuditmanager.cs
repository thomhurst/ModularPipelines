using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsAuditmanager
{
    public AwsAuditmanager(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AssociateAssessmentReportEvidenceFolder(AwsAuditmanagerAssociateAssessmentReportEvidenceFolderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchAssociateAssessmentReportEvidence(AwsAuditmanagerBatchAssociateAssessmentReportEvidenceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchCreateDelegationByAssessment(AwsAuditmanagerBatchCreateDelegationByAssessmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchDeleteDelegationByAssessment(AwsAuditmanagerBatchDeleteDelegationByAssessmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchDisassociateAssessmentReportEvidence(AwsAuditmanagerBatchDisassociateAssessmentReportEvidenceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchImportEvidenceToAssessmentControl(AwsAuditmanagerBatchImportEvidenceToAssessmentControlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAssessment(AwsAuditmanagerCreateAssessmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAssessmentFramework(AwsAuditmanagerCreateAssessmentFrameworkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAssessmentReport(AwsAuditmanagerCreateAssessmentReportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateControl(AwsAuditmanagerCreateControlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAssessment(AwsAuditmanagerDeleteAssessmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAssessmentFramework(AwsAuditmanagerDeleteAssessmentFrameworkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAssessmentFrameworkShare(AwsAuditmanagerDeleteAssessmentFrameworkShareOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAssessmentReport(AwsAuditmanagerDeleteAssessmentReportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteControl(AwsAuditmanagerDeleteControlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeregisterAccount(AwsAuditmanagerDeregisterAccountOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAuditmanagerDeregisterAccountOptions(), token);
    }

    public async Task<CommandResult> DeregisterOrganizationAdminAccount(AwsAuditmanagerDeregisterOrganizationAdminAccountOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAuditmanagerDeregisterOrganizationAdminAccountOptions(), token);
    }

    public async Task<CommandResult> DisassociateAssessmentReportEvidenceFolder(AwsAuditmanagerDisassociateAssessmentReportEvidenceFolderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAccountStatus(AwsAuditmanagerGetAccountStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAuditmanagerGetAccountStatusOptions(), token);
    }

    public async Task<CommandResult> GetAssessment(AwsAuditmanagerGetAssessmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAssessmentFramework(AwsAuditmanagerGetAssessmentFrameworkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAssessmentReportUrl(AwsAuditmanagerGetAssessmentReportUrlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetChangeLogs(AwsAuditmanagerGetChangeLogsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetControl(AwsAuditmanagerGetControlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDelegations(AwsAuditmanagerGetDelegationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAuditmanagerGetDelegationsOptions(), token);
    }

    public async Task<CommandResult> GetEvidence(AwsAuditmanagerGetEvidenceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetEvidenceByEvidenceFolder(AwsAuditmanagerGetEvidenceByEvidenceFolderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetEvidenceFileUploadUrl(AwsAuditmanagerGetEvidenceFileUploadUrlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetEvidenceFolder(AwsAuditmanagerGetEvidenceFolderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetEvidenceFoldersByAssessment(AwsAuditmanagerGetEvidenceFoldersByAssessmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetEvidenceFoldersByAssessmentControl(AwsAuditmanagerGetEvidenceFoldersByAssessmentControlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetInsights(AwsAuditmanagerGetInsightsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAuditmanagerGetInsightsOptions(), token);
    }

    public async Task<CommandResult> GetInsightsByAssessment(AwsAuditmanagerGetInsightsByAssessmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetOrganizationAdminAccount(AwsAuditmanagerGetOrganizationAdminAccountOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAuditmanagerGetOrganizationAdminAccountOptions(), token);
    }

    public async Task<CommandResult> GetServicesInScope(AwsAuditmanagerGetServicesInScopeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAuditmanagerGetServicesInScopeOptions(), token);
    }

    public async Task<CommandResult> GetSettings(AwsAuditmanagerGetSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAssessmentControlInsightsByControlDomain(AwsAuditmanagerListAssessmentControlInsightsByControlDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAssessmentFrameworkShareRequests(AwsAuditmanagerListAssessmentFrameworkShareRequestsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAssessmentFrameworks(AwsAuditmanagerListAssessmentFrameworksOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAssessmentReports(AwsAuditmanagerListAssessmentReportsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAuditmanagerListAssessmentReportsOptions(), token);
    }

    public async Task<CommandResult> ListAssessments(AwsAuditmanagerListAssessmentsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAuditmanagerListAssessmentsOptions(), token);
    }

    public async Task<CommandResult> ListControlDomainInsights(AwsAuditmanagerListControlDomainInsightsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAuditmanagerListControlDomainInsightsOptions(), token);
    }

    public async Task<CommandResult> ListControlDomainInsightsByAssessment(AwsAuditmanagerListControlDomainInsightsByAssessmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListControlInsightsByControlDomain(AwsAuditmanagerListControlInsightsByControlDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListControls(AwsAuditmanagerListControlsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListKeywordsForDataSource(AwsAuditmanagerListKeywordsForDataSourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListNotifications(AwsAuditmanagerListNotificationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAuditmanagerListNotificationsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsAuditmanagerListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterAccount(AwsAuditmanagerRegisterAccountOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAuditmanagerRegisterAccountOptions(), token);
    }

    public async Task<CommandResult> RegisterOrganizationAdminAccount(AwsAuditmanagerRegisterOrganizationAdminAccountOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartAssessmentFrameworkShare(AwsAuditmanagerStartAssessmentFrameworkShareOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsAuditmanagerTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsAuditmanagerUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAssessment(AwsAuditmanagerUpdateAssessmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAssessmentControl(AwsAuditmanagerUpdateAssessmentControlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAssessmentControlSetStatus(AwsAuditmanagerUpdateAssessmentControlSetStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAssessmentFramework(AwsAuditmanagerUpdateAssessmentFrameworkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAssessmentFrameworkShare(AwsAuditmanagerUpdateAssessmentFrameworkShareOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAssessmentStatus(AwsAuditmanagerUpdateAssessmentStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateControl(AwsAuditmanagerUpdateControlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSettings(AwsAuditmanagerUpdateSettingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAuditmanagerUpdateSettingsOptions(), token);
    }

    public async Task<CommandResult> ValidateAssessmentReportIntegrity(AwsAuditmanagerValidateAssessmentReportIntegrityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}