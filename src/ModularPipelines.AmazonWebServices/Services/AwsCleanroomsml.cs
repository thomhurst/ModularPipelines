using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsCleanroomsml
{
    public AwsCleanroomsml(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CreateAudienceModel(AwsCleanroomsmlCreateAudienceModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateConfiguredAudienceModel(AwsCleanroomsmlCreateConfiguredAudienceModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTrainingDataset(AwsCleanroomsmlCreateTrainingDatasetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAudienceGenerationJob(AwsCleanroomsmlDeleteAudienceGenerationJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAudienceModel(AwsCleanroomsmlDeleteAudienceModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteConfiguredAudienceModel(AwsCleanroomsmlDeleteConfiguredAudienceModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteConfiguredAudienceModelPolicy(AwsCleanroomsmlDeleteConfiguredAudienceModelPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTrainingDataset(AwsCleanroomsmlDeleteTrainingDatasetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAudienceGenerationJob(AwsCleanroomsmlGetAudienceGenerationJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAudienceModel(AwsCleanroomsmlGetAudienceModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetConfiguredAudienceModel(AwsCleanroomsmlGetConfiguredAudienceModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetConfiguredAudienceModelPolicy(AwsCleanroomsmlGetConfiguredAudienceModelPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTrainingDataset(AwsCleanroomsmlGetTrainingDatasetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAudienceExportJobs(AwsCleanroomsmlListAudienceExportJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCleanroomsmlListAudienceExportJobsOptions(), token);
    }

    public async Task<CommandResult> ListAudienceGenerationJobs(AwsCleanroomsmlListAudienceGenerationJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCleanroomsmlListAudienceGenerationJobsOptions(), token);
    }

    public async Task<CommandResult> ListAudienceModels(AwsCleanroomsmlListAudienceModelsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCleanroomsmlListAudienceModelsOptions(), token);
    }

    public async Task<CommandResult> ListConfiguredAudienceModels(AwsCleanroomsmlListConfiguredAudienceModelsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCleanroomsmlListConfiguredAudienceModelsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsCleanroomsmlListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTrainingDatasets(AwsCleanroomsmlListTrainingDatasetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCleanroomsmlListTrainingDatasetsOptions(), token);
    }

    public async Task<CommandResult> PutConfiguredAudienceModelPolicy(AwsCleanroomsmlPutConfiguredAudienceModelPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartAudienceExportJob(AwsCleanroomsmlStartAudienceExportJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartAudienceGenerationJob(AwsCleanroomsmlStartAudienceGenerationJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsCleanroomsmlTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsCleanroomsmlUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateConfiguredAudienceModel(AwsCleanroomsmlUpdateConfiguredAudienceModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}