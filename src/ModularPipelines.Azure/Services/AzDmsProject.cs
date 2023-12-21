using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms")]
public class AzDmsProject
{
    public AzDmsProject(
        AzDmsProjectCreate create,
        AzDmsProjectTask task,
        ICommand internalCommand
    )
    {
        CreateCommands = create;
        Task = task;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzDmsProjectCreate CreateCommands { get; }

    public AzDmsProjectTask Task { get; }

    public async Task<CommandResult> CheckName(AzDmsProjectCheckNameOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzDmsProjectCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDmsProjectDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzDmsProjectListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzDmsProjectShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}