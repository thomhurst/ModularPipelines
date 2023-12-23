using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsNeptuneGraph
{
    public AwsNeptuneGraph(
        AwsNeptuneGraphWait wait,
        ICommand internalCommand
    )
    {
        Wait = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AwsNeptuneGraphWait Wait { get; }

    public async Task<CommandResult> CancelImportTask(AwsNeptuneGraphCancelImportTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateGraph(AwsNeptuneGraphCreateGraphOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateGraphSnapshot(AwsNeptuneGraphCreateGraphSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateGraphUsingImportTask(AwsNeptuneGraphCreateGraphUsingImportTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePrivateGraphEndpoint(AwsNeptuneGraphCreatePrivateGraphEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteGraph(AwsNeptuneGraphDeleteGraphOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteGraphSnapshot(AwsNeptuneGraphDeleteGraphSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePrivateGraphEndpoint(AwsNeptuneGraphDeletePrivateGraphEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetGraph(AwsNeptuneGraphGetGraphOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetGraphSnapshot(AwsNeptuneGraphGetGraphSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetImportTask(AwsNeptuneGraphGetImportTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetPrivateGraphEndpoint(AwsNeptuneGraphGetPrivateGraphEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListGraphSnapshots(AwsNeptuneGraphListGraphSnapshotsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptuneGraphListGraphSnapshotsOptions(), token);
    }

    public async Task<CommandResult> ListGraphs(AwsNeptuneGraphListGraphsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptuneGraphListGraphsOptions(), token);
    }

    public async Task<CommandResult> ListImportTasks(AwsNeptuneGraphListImportTasksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptuneGraphListImportTasksOptions(), token);
    }

    public async Task<CommandResult> ListPrivateGraphEndpoints(AwsNeptuneGraphListPrivateGraphEndpointsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsNeptuneGraphListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResetGraph(AwsNeptuneGraphResetGraphOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RestoreGraphFromSnapshot(AwsNeptuneGraphRestoreGraphFromSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsNeptuneGraphTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsNeptuneGraphUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateGraph(AwsNeptuneGraphUpdateGraphOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}