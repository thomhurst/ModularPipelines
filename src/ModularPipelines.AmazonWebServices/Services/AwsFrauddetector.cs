using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsFrauddetector
{
    public AwsFrauddetector(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> BatchCreateVariable(AwsFrauddetectorBatchCreateVariableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetVariable(AwsFrauddetectorBatchGetVariableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelBatchImportJob(AwsFrauddetectorCancelBatchImportJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelBatchPredictionJob(AwsFrauddetectorCancelBatchPredictionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateBatchImportJob(AwsFrauddetectorCreateBatchImportJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateBatchPredictionJob(AwsFrauddetectorCreateBatchPredictionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDetectorVersion(AwsFrauddetectorCreateDetectorVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateList(AwsFrauddetectorCreateListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateModel(AwsFrauddetectorCreateModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateModelVersion(AwsFrauddetectorCreateModelVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRule(AwsFrauddetectorCreateRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateVariable(AwsFrauddetectorCreateVariableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteBatchImportJob(AwsFrauddetectorDeleteBatchImportJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteBatchPredictionJob(AwsFrauddetectorDeleteBatchPredictionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDetector(AwsFrauddetectorDeleteDetectorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDetectorVersion(AwsFrauddetectorDeleteDetectorVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteEntityType(AwsFrauddetectorDeleteEntityTypeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteEvent(AwsFrauddetectorDeleteEventOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteEventType(AwsFrauddetectorDeleteEventTypeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteEventsByEventType(AwsFrauddetectorDeleteEventsByEventTypeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteExternalModel(AwsFrauddetectorDeleteExternalModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteLabel(AwsFrauddetectorDeleteLabelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteList(AwsFrauddetectorDeleteListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteModel(AwsFrauddetectorDeleteModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteModelVersion(AwsFrauddetectorDeleteModelVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteOutcome(AwsFrauddetectorDeleteOutcomeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRule(AwsFrauddetectorDeleteRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVariable(AwsFrauddetectorDeleteVariableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDetector(AwsFrauddetectorDescribeDetectorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeModelVersions(AwsFrauddetectorDescribeModelVersionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFrauddetectorDescribeModelVersionsOptions(), token);
    }

    public async Task<CommandResult> GetBatchImportJobs(AwsFrauddetectorGetBatchImportJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFrauddetectorGetBatchImportJobsOptions(), token);
    }

    public async Task<CommandResult> GetBatchPredictionJobs(AwsFrauddetectorGetBatchPredictionJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFrauddetectorGetBatchPredictionJobsOptions(), token);
    }

    public async Task<CommandResult> GetDeleteEventsByEventTypeStatus(AwsFrauddetectorGetDeleteEventsByEventTypeStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDetectorVersion(AwsFrauddetectorGetDetectorVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDetectors(AwsFrauddetectorGetDetectorsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFrauddetectorGetDetectorsOptions(), token);
    }

    public async Task<CommandResult> GetEntityTypes(AwsFrauddetectorGetEntityTypesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFrauddetectorGetEntityTypesOptions(), token);
    }

    public async Task<CommandResult> GetEvent(AwsFrauddetectorGetEventOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetEventPrediction(AwsFrauddetectorGetEventPredictionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetEventPredictionMetadata(AwsFrauddetectorGetEventPredictionMetadataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetEventTypes(AwsFrauddetectorGetEventTypesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFrauddetectorGetEventTypesOptions(), token);
    }

    public async Task<CommandResult> GetExternalModels(AwsFrauddetectorGetExternalModelsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFrauddetectorGetExternalModelsOptions(), token);
    }

    public async Task<CommandResult> GetKmsEncryptionKey(AwsFrauddetectorGetKmsEncryptionKeyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFrauddetectorGetKmsEncryptionKeyOptions(), token);
    }

    public async Task<CommandResult> GetLabels(AwsFrauddetectorGetLabelsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFrauddetectorGetLabelsOptions(), token);
    }

    public async Task<CommandResult> GetListElements(AwsFrauddetectorGetListElementsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetListsMetadata(AwsFrauddetectorGetListsMetadataOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFrauddetectorGetListsMetadataOptions(), token);
    }

    public async Task<CommandResult> GetModelVersion(AwsFrauddetectorGetModelVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetModels(AwsFrauddetectorGetModelsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFrauddetectorGetModelsOptions(), token);
    }

    public async Task<CommandResult> GetOutcomes(AwsFrauddetectorGetOutcomesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFrauddetectorGetOutcomesOptions(), token);
    }

    public async Task<CommandResult> GetRules(AwsFrauddetectorGetRulesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetVariables(AwsFrauddetectorGetVariablesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFrauddetectorGetVariablesOptions(), token);
    }

    public async Task<CommandResult> ListEventPredictions(AwsFrauddetectorListEventPredictionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFrauddetectorListEventPredictionsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsFrauddetectorListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutDetector(AwsFrauddetectorPutDetectorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutEntityType(AwsFrauddetectorPutEntityTypeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutEventType(AwsFrauddetectorPutEventTypeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutExternalModel(AwsFrauddetectorPutExternalModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutKmsEncryptionKey(AwsFrauddetectorPutKmsEncryptionKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutLabel(AwsFrauddetectorPutLabelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutOutcome(AwsFrauddetectorPutOutcomeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SendEvent(AwsFrauddetectorSendEventOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsFrauddetectorTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsFrauddetectorUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDetectorVersion(AwsFrauddetectorUpdateDetectorVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDetectorVersionMetadata(AwsFrauddetectorUpdateDetectorVersionMetadataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDetectorVersionStatus(AwsFrauddetectorUpdateDetectorVersionStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateEventLabel(AwsFrauddetectorUpdateEventLabelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateList(AwsFrauddetectorUpdateListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateModel(AwsFrauddetectorUpdateModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateModelVersion(AwsFrauddetectorUpdateModelVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateModelVersionStatus(AwsFrauddetectorUpdateModelVersionStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateRuleMetadata(AwsFrauddetectorUpdateRuleMetadataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateRuleVersion(AwsFrauddetectorUpdateRuleVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateVariable(AwsFrauddetectorUpdateVariableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}