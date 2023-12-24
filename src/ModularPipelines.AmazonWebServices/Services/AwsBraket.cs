using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsBraket
{
    public AwsBraket(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CancelJob(AwsBraketCancelJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelQuantumTask(AwsBraketCancelQuantumTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateJob(AwsBraketCreateJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateQuantumTask(AwsBraketCreateQuantumTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDevice(AwsBraketGetDeviceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetJob(AwsBraketGetJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetQuantumTask(AwsBraketGetQuantumTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsBraketListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SearchDevices(AwsBraketSearchDevicesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SearchJobs(AwsBraketSearchJobsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SearchQuantumTasks(AwsBraketSearchQuantumTasksOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsBraketTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsBraketUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}