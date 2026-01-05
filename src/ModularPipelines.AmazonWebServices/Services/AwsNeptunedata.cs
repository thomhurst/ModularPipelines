using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    public async Task<CommandResult> CancelGremlinQuery(AwsNeptunedataCancelGremlinQueryOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CancelLoaderJob(AwsNeptunedataCancelLoaderJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CancelMlDataProcessingJob(AwsNeptunedataCancelMlDataProcessingJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CancelMlModelTrainingJob(AwsNeptunedataCancelMlModelTrainingJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CancelMlModelTransformJob(AwsNeptunedataCancelMlModelTransformJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CancelOpenCypherQuery(AwsNeptunedataCancelOpenCypherQueryOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateMlEndpoint(AwsNeptunedataCreateMlEndpointOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataCreateMlEndpointOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteMlEndpoint(AwsNeptunedataDeleteMlEndpointOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeletePropertygraphStatistics(AwsNeptunedataDeletePropertygraphStatisticsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataDeletePropertygraphStatisticsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteSparqlStatistics(AwsNeptunedataDeleteSparqlStatisticsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataDeleteSparqlStatisticsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ExecuteFastReset(AwsNeptunedataExecuteFastResetOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ExecuteGremlinExplainQuery(AwsNeptunedataExecuteGremlinExplainQueryOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ExecuteGremlinProfileQuery(AwsNeptunedataExecuteGremlinProfileQueryOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ExecuteGremlinQuery(AwsNeptunedataExecuteGremlinQueryOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ExecuteOpenCypherExplainQuery(AwsNeptunedataExecuteOpenCypherExplainQueryOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ExecuteOpenCypherQuery(AwsNeptunedataExecuteOpenCypherQueryOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetEngineStatus(AwsNeptunedataGetEngineStatusOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataGetEngineStatusOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetGremlinQueryStatus(AwsNeptunedataGetGremlinQueryStatusOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetLoaderJobStatus(AwsNeptunedataGetLoaderJobStatusOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetMlDataProcessingJob(AwsNeptunedataGetMlDataProcessingJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetMlEndpoint(AwsNeptunedataGetMlEndpointOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetMlModelTrainingJob(AwsNeptunedataGetMlModelTrainingJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetMlModelTransformJob(AwsNeptunedataGetMlModelTransformJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetOpenCypherQueryStatus(AwsNeptunedataGetOpenCypherQueryStatusOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetPropertygraphStatistics(AwsNeptunedataGetPropertygraphStatisticsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataGetPropertygraphStatisticsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetPropertygraphStream(AwsNeptunedataGetPropertygraphStreamOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataGetPropertygraphStreamOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetPropertygraphSummary(AwsNeptunedataGetPropertygraphSummaryOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataGetPropertygraphSummaryOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetRdfGraphSummary(AwsNeptunedataGetRdfGraphSummaryOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataGetRdfGraphSummaryOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetSparqlStatistics(AwsNeptunedataGetSparqlStatisticsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataGetSparqlStatisticsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetSparqlStream(AwsNeptunedataGetSparqlStreamOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataGetSparqlStreamOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListGremlinQueries(AwsNeptunedataListGremlinQueriesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataListGremlinQueriesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListLoaderJobs(AwsNeptunedataListLoaderJobsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataListLoaderJobsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListMlDataProcessingJobs(AwsNeptunedataListMlDataProcessingJobsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataListMlDataProcessingJobsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListMlEndpoints(AwsNeptunedataListMlEndpointsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataListMlEndpointsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListMlModelTrainingJobs(AwsNeptunedataListMlModelTrainingJobsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataListMlModelTrainingJobsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListMlModelTransformJobs(AwsNeptunedataListMlModelTransformJobsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataListMlModelTransformJobsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListOpenCypherQueries(AwsNeptunedataListOpenCypherQueriesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataListOpenCypherQueriesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ManagePropertygraphStatistics(AwsNeptunedataManagePropertygraphStatisticsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataManagePropertygraphStatisticsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ManageSparqlStatistics(AwsNeptunedataManageSparqlStatisticsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptunedataManageSparqlStatisticsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartLoaderJob(AwsNeptunedataStartLoaderJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartMlDataProcessingJob(AwsNeptunedataStartMlDataProcessingJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartMlModelTrainingJob(AwsNeptunedataStartMlModelTrainingJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartMlModelTransformJob(AwsNeptunedataStartMlModelTransformJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}