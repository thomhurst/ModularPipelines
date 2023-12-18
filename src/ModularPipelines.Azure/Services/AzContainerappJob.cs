using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp")]
public class AzContainerappJob
{
    public AzContainerappJob(
        AzContainerappJobExecution execution,
        AzContainerappJobIdentity identity,
        AzContainerappJobSecret secret,
        ICommand internalCommand
    )
    {
        Execution = execution;
        Identity = identity;
        Secret = secret;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzContainerappJobExecution Execution { get; }

    public AzContainerappJobIdentity Identity { get; }

    public AzContainerappJobSecret Secret { get; }

    public async Task<CommandResult> Create(AzContainerappJobCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzContainerappJobDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappJobDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzContainerappJobListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappJobListOptions(), token);
    }

    public async Task<CommandResult> Show(AzContainerappJobShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappJobShowOptions(), token);
    }

    public async Task<CommandResult> Start(AzContainerappJobStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappJobStartOptions(), token);
    }

    public async Task<CommandResult> Stop(AzContainerappJobStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappJobStopOptions(), token);
    }

    public async Task<CommandResult> Update(AzContainerappJobUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappJobUpdateOptions(), token);
    }
}