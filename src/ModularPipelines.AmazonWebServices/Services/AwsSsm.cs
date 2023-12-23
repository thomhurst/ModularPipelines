using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsSsm
{
    public AwsSsm(
        AwsSsmWait wait,
        ICommand internalCommand
    )
    {
        Wait = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AwsSsmWait Wait { get; }

    public async Task<CommandResult> AddTagsToResource(AwsSsmAddTagsToResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateOpsItemRelatedItem(AwsSsmAssociateOpsItemRelatedItemOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelCommand(AwsSsmCancelCommandOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelMaintenanceWindowExecution(AwsSsmCancelMaintenanceWindowExecutionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateActivation(AwsSsmCreateActivationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAssociation(AwsSsmCreateAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAssociationBatch(AwsSsmCreateAssociationBatchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDocument(AwsSsmCreateDocumentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateMaintenanceWindow(AwsSsmCreateMaintenanceWindowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateOpsItem(AwsSsmCreateOpsItemOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateOpsMetadata(AwsSsmCreateOpsMetadataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePatchBaseline(AwsSsmCreatePatchBaselineOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateResourceDataSync(AwsSsmCreateResourceDataSyncOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteActivation(AwsSsmDeleteActivationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAssociation(AwsSsmDeleteAssociationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSsmDeleteAssociationOptions(), token);
    }

    public async Task<CommandResult> DeleteDocument(AwsSsmDeleteDocumentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteInventory(AwsSsmDeleteInventoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteMaintenanceWindow(AwsSsmDeleteMaintenanceWindowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteOpsItem(AwsSsmDeleteOpsItemOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteOpsMetadata(AwsSsmDeleteOpsMetadataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteParameter(AwsSsmDeleteParameterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteParameters(AwsSsmDeleteParametersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePatchBaseline(AwsSsmDeletePatchBaselineOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteResourceDataSync(AwsSsmDeleteResourceDataSyncOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteResourcePolicy(AwsSsmDeleteResourcePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeregisterManagedInstance(AwsSsmDeregisterManagedInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeregisterPatchBaselineForPatchGroup(AwsSsmDeregisterPatchBaselineForPatchGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeregisterTargetFromMaintenanceWindow(AwsSsmDeregisterTargetFromMaintenanceWindowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeregisterTaskFromMaintenanceWindow(AwsSsmDeregisterTaskFromMaintenanceWindowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeActivations(AwsSsmDescribeActivationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSsmDescribeActivationsOptions(), token);
    }

    public async Task<CommandResult> DescribeAssociation(AwsSsmDescribeAssociationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSsmDescribeAssociationOptions(), token);
    }

    public async Task<CommandResult> DescribeAssociationExecutionTargets(AwsSsmDescribeAssociationExecutionTargetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAssociationExecutions(AwsSsmDescribeAssociationExecutionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAutomationExecutions(AwsSsmDescribeAutomationExecutionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSsmDescribeAutomationExecutionsOptions(), token);
    }

    public async Task<CommandResult> DescribeAutomationStepExecutions(AwsSsmDescribeAutomationStepExecutionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAvailablePatches(AwsSsmDescribeAvailablePatchesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSsmDescribeAvailablePatchesOptions(), token);
    }

    public async Task<CommandResult> DescribeDocument(AwsSsmDescribeDocumentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDocumentPermission(AwsSsmDescribeDocumentPermissionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeEffectiveInstanceAssociations(AwsSsmDescribeEffectiveInstanceAssociationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeEffectivePatchesForPatchBaseline(AwsSsmDescribeEffectivePatchesForPatchBaselineOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeInstanceAssociationsStatus(AwsSsmDescribeInstanceAssociationsStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeInstanceInformation(AwsSsmDescribeInstanceInformationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSsmDescribeInstanceInformationOptions(), token);
    }

    public async Task<CommandResult> DescribeInstancePatchStates(AwsSsmDescribeInstancePatchStatesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeInstancePatchStatesForPatchGroup(AwsSsmDescribeInstancePatchStatesForPatchGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeInstancePatches(AwsSsmDescribeInstancePatchesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeInventoryDeletions(AwsSsmDescribeInventoryDeletionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSsmDescribeInventoryDeletionsOptions(), token);
    }

    public async Task<CommandResult> DescribeMaintenanceWindowExecutionTaskInvocations(AwsSsmDescribeMaintenanceWindowExecutionTaskInvocationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeMaintenanceWindowExecutionTasks(AwsSsmDescribeMaintenanceWindowExecutionTasksOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeMaintenanceWindowExecutions(AwsSsmDescribeMaintenanceWindowExecutionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeMaintenanceWindowSchedule(AwsSsmDescribeMaintenanceWindowScheduleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSsmDescribeMaintenanceWindowScheduleOptions(), token);
    }

    public async Task<CommandResult> DescribeMaintenanceWindowTargets(AwsSsmDescribeMaintenanceWindowTargetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeMaintenanceWindowTasks(AwsSsmDescribeMaintenanceWindowTasksOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeMaintenanceWindows(AwsSsmDescribeMaintenanceWindowsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSsmDescribeMaintenanceWindowsOptions(), token);
    }

    public async Task<CommandResult> DescribeMaintenanceWindowsForTarget(AwsSsmDescribeMaintenanceWindowsForTargetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeOpsItems(AwsSsmDescribeOpsItemsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSsmDescribeOpsItemsOptions(), token);
    }

    public async Task<CommandResult> DescribeParameters(AwsSsmDescribeParametersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSsmDescribeParametersOptions(), token);
    }

    public async Task<CommandResult> DescribePatchBaselines(AwsSsmDescribePatchBaselinesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSsmDescribePatchBaselinesOptions(), token);
    }

    public async Task<CommandResult> DescribePatchGroupState(AwsSsmDescribePatchGroupStateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribePatchGroups(AwsSsmDescribePatchGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSsmDescribePatchGroupsOptions(), token);
    }

    public async Task<CommandResult> DescribePatchProperties(AwsSsmDescribePatchPropertiesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeSessions(AwsSsmDescribeSessionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateOpsItemRelatedItem(AwsSsmDisassociateOpsItemRelatedItemOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAutomationExecution(AwsSsmGetAutomationExecutionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCalendarState(AwsSsmGetCalendarStateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCommandInvocation(AwsSsmGetCommandInvocationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetConnectionStatus(AwsSsmGetConnectionStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDefaultPatchBaseline(AwsSsmGetDefaultPatchBaselineOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSsmGetDefaultPatchBaselineOptions(), token);
    }

    public async Task<CommandResult> GetDeployablePatchSnapshotForInstance(AwsSsmGetDeployablePatchSnapshotForInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDocument(AwsSsmGetDocumentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetInventory(AwsSsmGetInventoryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSsmGetInventoryOptions(), token);
    }

    public async Task<CommandResult> GetInventorySchema(AwsSsmGetInventorySchemaOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSsmGetInventorySchemaOptions(), token);
    }

    public async Task<CommandResult> GetMaintenanceWindow(AwsSsmGetMaintenanceWindowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMaintenanceWindowExecution(AwsSsmGetMaintenanceWindowExecutionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMaintenanceWindowExecutionTask(AwsSsmGetMaintenanceWindowExecutionTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMaintenanceWindowExecutionTaskInvocation(AwsSsmGetMaintenanceWindowExecutionTaskInvocationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMaintenanceWindowTask(AwsSsmGetMaintenanceWindowTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetOpsItem(AwsSsmGetOpsItemOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetOpsMetadata(AwsSsmGetOpsMetadataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetOpsSummary(AwsSsmGetOpsSummaryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSsmGetOpsSummaryOptions(), token);
    }

    public async Task<CommandResult> GetParameter(AwsSsmGetParameterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetParameterHistory(AwsSsmGetParameterHistoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetParameters(AwsSsmGetParametersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetParametersByPath(AwsSsmGetParametersByPathOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetPatchBaseline(AwsSsmGetPatchBaselineOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetPatchBaselineForPatchGroup(AwsSsmGetPatchBaselineForPatchGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetResourcePolicies(AwsSsmGetResourcePoliciesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetServiceSetting(AwsSsmGetServiceSettingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> LabelParameterVersion(AwsSsmLabelParameterVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAssociationVersions(AwsSsmListAssociationVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAssociations(AwsSsmListAssociationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSsmListAssociationsOptions(), token);
    }

    public async Task<CommandResult> ListCommandInvocations(AwsSsmListCommandInvocationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSsmListCommandInvocationsOptions(), token);
    }

    public async Task<CommandResult> ListCommands(AwsSsmListCommandsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSsmListCommandsOptions(), token);
    }

    public async Task<CommandResult> ListComplianceItems(AwsSsmListComplianceItemsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSsmListComplianceItemsOptions(), token);
    }

    public async Task<CommandResult> ListComplianceSummaries(AwsSsmListComplianceSummariesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSsmListComplianceSummariesOptions(), token);
    }

    public async Task<CommandResult> ListDocumentMetadataHistory(AwsSsmListDocumentMetadataHistoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListDocumentVersions(AwsSsmListDocumentVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListDocuments(AwsSsmListDocumentsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSsmListDocumentsOptions(), token);
    }

    public async Task<CommandResult> ListInventoryEntries(AwsSsmListInventoryEntriesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListOpsItemEvents(AwsSsmListOpsItemEventsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSsmListOpsItemEventsOptions(), token);
    }

    public async Task<CommandResult> ListOpsItemRelatedItems(AwsSsmListOpsItemRelatedItemsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSsmListOpsItemRelatedItemsOptions(), token);
    }

    public async Task<CommandResult> ListOpsMetadata(AwsSsmListOpsMetadataOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSsmListOpsMetadataOptions(), token);
    }

    public async Task<CommandResult> ListResourceComplianceSummaries(AwsSsmListResourceComplianceSummariesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSsmListResourceComplianceSummariesOptions(), token);
    }

    public async Task<CommandResult> ListResourceDataSync(AwsSsmListResourceDataSyncOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSsmListResourceDataSyncOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsSsmListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyDocumentPermission(AwsSsmModifyDocumentPermissionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutComplianceItems(AwsSsmPutComplianceItemsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutInventory(AwsSsmPutInventoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutParameter(AwsSsmPutParameterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutResourcePolicy(AwsSsmPutResourcePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterDefaultPatchBaseline(AwsSsmRegisterDefaultPatchBaselineOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterPatchBaselineForPatchGroup(AwsSsmRegisterPatchBaselineForPatchGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterTargetWithMaintenanceWindow(AwsSsmRegisterTargetWithMaintenanceWindowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterTaskWithMaintenanceWindow(AwsSsmRegisterTaskWithMaintenanceWindowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveTagsFromResource(AwsSsmRemoveTagsFromResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResetServiceSetting(AwsSsmResetServiceSettingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResumeSession(AwsSsmResumeSessionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SendAutomationSignal(AwsSsmSendAutomationSignalOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SendCommand(AwsSsmSendCommandOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartAssociationsOnce(AwsSsmStartAssociationsOnceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartAutomationExecution(AwsSsmStartAutomationExecutionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartChangeRequestExecution(AwsSsmStartChangeRequestExecutionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartSession(AwsSsmStartSessionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopAutomationExecution(AwsSsmStopAutomationExecutionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TerminateSession(AwsSsmTerminateSessionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UnlabelParameterVersion(AwsSsmUnlabelParameterVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAssociation(AwsSsmUpdateAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAssociationStatus(AwsSsmUpdateAssociationStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDocument(AwsSsmUpdateDocumentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDocumentDefaultVersion(AwsSsmUpdateDocumentDefaultVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDocumentMetadata(AwsSsmUpdateDocumentMetadataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateMaintenanceWindow(AwsSsmUpdateMaintenanceWindowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateMaintenanceWindowTarget(AwsSsmUpdateMaintenanceWindowTargetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateMaintenanceWindowTask(AwsSsmUpdateMaintenanceWindowTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateManagedInstanceRole(AwsSsmUpdateManagedInstanceRoleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateOpsItem(AwsSsmUpdateOpsItemOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateOpsMetadata(AwsSsmUpdateOpsMetadataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdatePatchBaseline(AwsSsmUpdatePatchBaselineOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateResourceDataSync(AwsSsmUpdateResourceDataSyncOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateServiceSetting(AwsSsmUpdateServiceSettingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}