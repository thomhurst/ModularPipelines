using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsMachinelearning
{
    public AwsMachinelearning(
        AwsMachinelearningWait wait,
        ICommand internalCommand
    )
    {
        Wait = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AwsMachinelearningWait Wait { get; }

    public async Task<CommandResult> AddTags(AwsMachinelearningAddTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateBatchPrediction(AwsMachinelearningCreateBatchPredictionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDataSourceFromRds(AwsMachinelearningCreateDataSourceFromRdsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDataSourceFromRedshift(AwsMachinelearningCreateDataSourceFromRedshiftOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDataSourceFromS3(AwsMachinelearningCreateDataSourceFromS3Options options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateEvaluation(AwsMachinelearningCreateEvaluationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateMlModel(AwsMachinelearningCreateMlModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRealtimeEndpoint(AwsMachinelearningCreateRealtimeEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteBatchPrediction(AwsMachinelearningDeleteBatchPredictionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDataSource(AwsMachinelearningDeleteDataSourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteEvaluation(AwsMachinelearningDeleteEvaluationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteMlModel(AwsMachinelearningDeleteMlModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRealtimeEndpoint(AwsMachinelearningDeleteRealtimeEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTags(AwsMachinelearningDeleteTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeBatchPredictions(AwsMachinelearningDescribeBatchPredictionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMachinelearningDescribeBatchPredictionsOptions(), token);
    }

    public async Task<CommandResult> DescribeDataSources(AwsMachinelearningDescribeDataSourcesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMachinelearningDescribeDataSourcesOptions(), token);
    }

    public async Task<CommandResult> DescribeEvaluations(AwsMachinelearningDescribeEvaluationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMachinelearningDescribeEvaluationsOptions(), token);
    }

    public async Task<CommandResult> DescribeMlModels(AwsMachinelearningDescribeMlModelsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMachinelearningDescribeMlModelsOptions(), token);
    }

    public async Task<CommandResult> DescribeTags(AwsMachinelearningDescribeTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetBatchPrediction(AwsMachinelearningGetBatchPredictionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDataSource(AwsMachinelearningGetDataSourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetEvaluation(AwsMachinelearningGetEvaluationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMlModel(AwsMachinelearningGetMlModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Predict(AwsMachinelearningPredictOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateBatchPrediction(AwsMachinelearningUpdateBatchPredictionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDataSource(AwsMachinelearningUpdateDataSourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateEvaluation(AwsMachinelearningUpdateEvaluationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateMlModel(AwsMachinelearningUpdateMlModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}