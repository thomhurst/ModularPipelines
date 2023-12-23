using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsBackup
{
    public AwsBackup(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CancelLegalHold(AwsBackupCancelLegalHoldOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateBackupPlan(AwsBackupCreateBackupPlanOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateBackupSelection(AwsBackupCreateBackupSelectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateBackupVault(AwsBackupCreateBackupVaultOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateFramework(AwsBackupCreateFrameworkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLegalHold(AwsBackupCreateLegalHoldOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLogicallyAirGappedBackupVault(AwsBackupCreateLogicallyAirGappedBackupVaultOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateReportPlan(AwsBackupCreateReportPlanOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRestoreTestingPlan(AwsBackupCreateRestoreTestingPlanOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRestoreTestingSelection(AwsBackupCreateRestoreTestingSelectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteBackupPlan(AwsBackupDeleteBackupPlanOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteBackupSelection(AwsBackupDeleteBackupSelectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteBackupVault(AwsBackupDeleteBackupVaultOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteBackupVaultAccessPolicy(AwsBackupDeleteBackupVaultAccessPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteBackupVaultLockConfiguration(AwsBackupDeleteBackupVaultLockConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteBackupVaultNotifications(AwsBackupDeleteBackupVaultNotificationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteFramework(AwsBackupDeleteFrameworkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRecoveryPoint(AwsBackupDeleteRecoveryPointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteReportPlan(AwsBackupDeleteReportPlanOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRestoreTestingPlan(AwsBackupDeleteRestoreTestingPlanOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRestoreTestingSelection(AwsBackupDeleteRestoreTestingSelectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeBackupJob(AwsBackupDescribeBackupJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeBackupVault(AwsBackupDescribeBackupVaultOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeCopyJob(AwsBackupDescribeCopyJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeFramework(AwsBackupDescribeFrameworkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeGlobalSettings(AwsBackupDescribeGlobalSettingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsBackupDescribeGlobalSettingsOptions(), token);
    }

    public async Task<CommandResult> DescribeProtectedResource(AwsBackupDescribeProtectedResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeRecoveryPoint(AwsBackupDescribeRecoveryPointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeRegionSettings(AwsBackupDescribeRegionSettingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsBackupDescribeRegionSettingsOptions(), token);
    }

    public async Task<CommandResult> DescribeReportJob(AwsBackupDescribeReportJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeReportPlan(AwsBackupDescribeReportPlanOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeRestoreJob(AwsBackupDescribeRestoreJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateRecoveryPoint(AwsBackupDisassociateRecoveryPointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateRecoveryPointFromParent(AwsBackupDisassociateRecoveryPointFromParentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ExportBackupPlanTemplate(AwsBackupExportBackupPlanTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetBackupPlan(AwsBackupGetBackupPlanOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetBackupPlanFromJson(AwsBackupGetBackupPlanFromJsonOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetBackupPlanFromTemplate(AwsBackupGetBackupPlanFromTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetBackupSelection(AwsBackupGetBackupSelectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetBackupVaultAccessPolicy(AwsBackupGetBackupVaultAccessPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetBackupVaultNotifications(AwsBackupGetBackupVaultNotificationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLegalHold(AwsBackupGetLegalHoldOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRecoveryPointRestoreMetadata(AwsBackupGetRecoveryPointRestoreMetadataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRestoreJobMetadata(AwsBackupGetRestoreJobMetadataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRestoreTestingInferredMetadata(AwsBackupGetRestoreTestingInferredMetadataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRestoreTestingPlan(AwsBackupGetRestoreTestingPlanOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRestoreTestingSelection(AwsBackupGetRestoreTestingSelectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSupportedResourceTypes(AwsBackupGetSupportedResourceTypesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsBackupGetSupportedResourceTypesOptions(), token);
    }

    public async Task<CommandResult> ListBackupJobSummaries(AwsBackupListBackupJobSummariesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsBackupListBackupJobSummariesOptions(), token);
    }

    public async Task<CommandResult> ListBackupJobs(AwsBackupListBackupJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsBackupListBackupJobsOptions(), token);
    }

    public async Task<CommandResult> ListBackupPlanTemplates(AwsBackupListBackupPlanTemplatesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsBackupListBackupPlanTemplatesOptions(), token);
    }

    public async Task<CommandResult> ListBackupPlanVersions(AwsBackupListBackupPlanVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListBackupPlans(AwsBackupListBackupPlansOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsBackupListBackupPlansOptions(), token);
    }

    public async Task<CommandResult> ListBackupSelections(AwsBackupListBackupSelectionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListBackupVaults(AwsBackupListBackupVaultsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsBackupListBackupVaultsOptions(), token);
    }

    public async Task<CommandResult> ListCopyJobSummaries(AwsBackupListCopyJobSummariesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsBackupListCopyJobSummariesOptions(), token);
    }

    public async Task<CommandResult> ListCopyJobs(AwsBackupListCopyJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsBackupListCopyJobsOptions(), token);
    }

    public async Task<CommandResult> ListFrameworks(AwsBackupListFrameworksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsBackupListFrameworksOptions(), token);
    }

    public async Task<CommandResult> ListLegalHolds(AwsBackupListLegalHoldsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsBackupListLegalHoldsOptions(), token);
    }

    public async Task<CommandResult> ListProtectedResources(AwsBackupListProtectedResourcesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsBackupListProtectedResourcesOptions(), token);
    }

    public async Task<CommandResult> ListProtectedResourcesByBackupVault(AwsBackupListProtectedResourcesByBackupVaultOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListRecoveryPointsByBackupVault(AwsBackupListRecoveryPointsByBackupVaultOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListRecoveryPointsByLegalHold(AwsBackupListRecoveryPointsByLegalHoldOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListRecoveryPointsByResource(AwsBackupListRecoveryPointsByResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListReportJobs(AwsBackupListReportJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsBackupListReportJobsOptions(), token);
    }

    public async Task<CommandResult> ListReportPlans(AwsBackupListReportPlansOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsBackupListReportPlansOptions(), token);
    }

    public async Task<CommandResult> ListRestoreJobSummaries(AwsBackupListRestoreJobSummariesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsBackupListRestoreJobSummariesOptions(), token);
    }

    public async Task<CommandResult> ListRestoreJobs(AwsBackupListRestoreJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsBackupListRestoreJobsOptions(), token);
    }

    public async Task<CommandResult> ListRestoreJobsByProtectedResource(AwsBackupListRestoreJobsByProtectedResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListRestoreTestingPlans(AwsBackupListRestoreTestingPlansOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsBackupListRestoreTestingPlansOptions(), token);
    }

    public async Task<CommandResult> ListRestoreTestingSelections(AwsBackupListRestoreTestingSelectionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTags(AwsBackupListTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutBackupVaultAccessPolicy(AwsBackupPutBackupVaultAccessPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutBackupVaultLockConfiguration(AwsBackupPutBackupVaultLockConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutBackupVaultNotifications(AwsBackupPutBackupVaultNotificationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutRestoreValidationResult(AwsBackupPutRestoreValidationResultOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartBackupJob(AwsBackupStartBackupJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartCopyJob(AwsBackupStartCopyJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartReportJob(AwsBackupStartReportJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartRestoreJob(AwsBackupStartRestoreJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopBackupJob(AwsBackupStopBackupJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsBackupTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsBackupUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateBackupPlan(AwsBackupUpdateBackupPlanOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateFramework(AwsBackupUpdateFrameworkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateGlobalSettings(AwsBackupUpdateGlobalSettingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsBackupUpdateGlobalSettingsOptions(), token);
    }

    public async Task<CommandResult> UpdateRecoveryPointLifecycle(AwsBackupUpdateRecoveryPointLifecycleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateRegionSettings(AwsBackupUpdateRegionSettingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsBackupUpdateRegionSettingsOptions(), token);
    }

    public async Task<CommandResult> UpdateReportPlan(AwsBackupUpdateReportPlanOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateRestoreTestingPlan(AwsBackupUpdateRestoreTestingPlanOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateRestoreTestingSelection(AwsBackupUpdateRestoreTestingSelectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}