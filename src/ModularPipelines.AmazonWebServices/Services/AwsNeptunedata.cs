using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsNeptunedata
{
    public AwsNeptunedata(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CancelGremlinQuery(AwsNeptunedataCancelGremlinQueryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelLoaderJob(AwsNeptunedataCancelLoaderJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelMlDataProcessingJob(AwsNeptunedataCancelMlDataProcessingJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelMlModelTrainingJob(AwsNeptunedataCancelMlModelTrainingJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelMlModelTransformJob(AwsNeptunedataCancelMlModelTransformJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelOpenCypherQuery(AwsNeptunedataCancelOpenCypherQueryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateMlEndpoint(AwsNeptunedataCreateMlEndpointOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataCreateMlEndpointOptions(), token);
    }

    public async Task<CommandResult> DeleteMlEndpoint(AwsNeptunedataDeleteMlEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePropertygraphStatistics(AwsNeptunedataDeletePropertygraphStatisticsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataDeletePropertygraphStatisticsOptions(), token);
    }

    public async Task<CommandResult> DeleteSparqlStatistics(AwsNeptunedataDeleteSparqlStatisticsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataDeleteSparqlStatisticsOptions(), token);
    }

    public async Task<CommandResult> ExecuteFastReset(AwsNeptunedataExecuteFastResetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ExecuteGremlinExplainQuery(AwsNeptunedataExecuteGremlinExplainQueryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ExecuteGremlinProfileQuery(AwsNeptunedataExecuteGremlinProfileQueryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ExecuteGremlinQuery(AwsNeptunedataExecuteGremlinQueryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ExecuteOpenCypherExplainQuery(AwsNeptunedataExecuteOpenCypherExplainQueryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ExecuteOpenCypherQuery(AwsNeptunedataExecuteOpenCypherQueryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetEngineStatus(AwsNeptunedataGetEngineStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataGetEngineStatusOptions(), token);
    }

    public async Task<CommandResult> GetGremlinQueryStatus(AwsNeptunedataGetGremlinQueryStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLoaderJobStatus(AwsNeptunedataGetLoaderJobStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMlDataProcessingJob(AwsNeptunedataGetMlDataProcessingJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMlEndpoint(AwsNeptunedataGetMlEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMlModelTrainingJob(AwsNeptunedataGetMlModelTrainingJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMlModelTransformJob(AwsNeptunedataGetMlModelTransformJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetOpenCypherQueryStatus(AwsNeptunedataGetOpenCypherQueryStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetPropertygraphStatistics(AwsNeptunedataGetPropertygraphStatisticsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataGetPropertygraphStatisticsOptions(), token);
    }

    public async Task<CommandResult> GetPropertygraphStream(AwsNeptunedataGetPropertygraphStreamOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataGetPropertygraphStreamOptions(), token);
    }

    public async Task<CommandResult> GetPropertygraphSummary(AwsNeptunedataGetPropertygraphSummaryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataGetPropertygraphSummaryOptions(), token);
    }

    public async Task<CommandResult> GetRdfGraphSummary(AwsNeptunedataGetRdfGraphSummaryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataGetRdfGraphSummaryOptions(), token);
    }

    public async Task<CommandResult> GetSparqlStatistics(AwsNeptunedataGetSparqlStatisticsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataGetSparqlStatisticsOptions(), token);
    }

    public async Task<CommandResult> GetSparqlStream(AwsNeptunedataGetSparqlStreamOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataGetSparqlStreamOptions(), token);
    }

    public async Task<CommandResult> ListGremlinQueries(AwsNeptunedataListGremlinQueriesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataListGremlinQueriesOptions(), token);
    }

    public async Task<CommandResult> ListLoaderJobs(AwsNeptunedataListLoaderJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataListLoaderJobsOptions(), token);
    }

    public async Task<CommandResult> ListMlDataProcessingJobs(AwsNeptunedataListMlDataProcessingJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataListMlDataProcessingJobsOptions(), token);
    }

    public async Task<CommandResult> ListMlEndpoints(AwsNeptunedataListMlEndpointsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataListMlEndpointsOptions(), token);
    }

    public async Task<CommandResult> ListMlModelTrainingJobs(AwsNeptunedataListMlModelTrainingJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataListMlModelTrainingJobsOptions(), token);
    }

    public async Task<CommandResult> ListMlModelTransformJobs(AwsNeptunedataListMlModelTransformJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataListMlModelTransformJobsOptions(), token);
    }

    public async Task<CommandResult> ListOpenCypherQueries(AwsNeptunedataListOpenCypherQueriesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataListOpenCypherQueriesOptions(), token);
    }

    public async Task<CommandResult> ManagePropertygraphStatistics(AwsNeptunedataManagePropertygraphStatisticsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataManagePropertygraphStatisticsOptions(), token);
    }

    public async Task<CommandResult> ManageSparqlStatistics(AwsNeptunedataManageSparqlStatisticsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataManageSparqlStatisticsOptions(), token);
    }

    public async Task<CommandResult> StartLoaderJob(AwsNeptunedataStartLoaderJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartMlDataProcessingJob(AwsNeptunedataStartMlDataProcessingJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartMlModelTrainingJob(AwsNeptunedataStartMlModelTrainingJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartMlModelTransformJob(AwsNeptunedataStartMlModelTransformJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}