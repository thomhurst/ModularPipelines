using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzDiskAccess
{
    public AzDiskAccess(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzDiskAccessCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzDiskAccessDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDiskAccessDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzDiskAccessListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDiskAccessListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzDiskAccessShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDiskAccessShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzDiskAccessUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDiskAccessUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzDiskAccessWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDiskAccessWaitOptions(), cancellationToken: token);
    }
}