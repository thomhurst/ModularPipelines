using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzStorageMover
{
    public AzStorageMover(
        AzStorageMoverAgent agent,
        AzStorageMoverEndpoint endpoint,
        AzStorageMoverJobDefinition jobDefinition,
        AzStorageMoverJobRun jobRun,
        AzStorageMoverProject project,
        ICommand internalCommand
    )
    {
        Agent = agent;
        Endpoint = endpoint;
        JobDefinition = jobDefinition;
        JobRun = jobRun;
        Project = project;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzStorageMoverAgent Agent { get; }

    public AzStorageMoverEndpoint Endpoint { get; }

    public AzStorageMoverJobDefinition JobDefinition { get; }

    public AzStorageMoverJobRun JobRun { get; }

    public AzStorageMoverProject Project { get; }

    public async Task<CommandResult> Create(AzStorageMoverCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzStorageMoverDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageMoverDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzStorageMoverListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageMoverListOptions(), token);
    }

    public async Task<CommandResult> Show(AzStorageMoverShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageMoverShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzStorageMoverUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageMoverUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzStorageMoverWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageMoverWaitOptions(), token);
    }
}