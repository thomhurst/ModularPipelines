using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("disk-pool")]
public class AzDiskPoolIscsiTarget
{
    public AzDiskPoolIscsiTarget(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzDiskPoolIscsiTargetCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDiskPoolIscsiTargetDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDiskPoolIscsiTargetDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzDiskPoolIscsiTargetListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzDiskPoolIscsiTargetShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDiskPoolIscsiTargetShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzDiskPoolIscsiTargetUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDiskPoolIscsiTargetUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzDiskPoolIscsiTargetWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDiskPoolIscsiTargetWaitOptions(), token);
    }
}