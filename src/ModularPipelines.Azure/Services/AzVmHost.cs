using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("vm")]
public class AzVmHost
{
    public AzVmHost(
        AzVmHostGroup group,
        ICommand internalCommand
    )
    {
        Group = group;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzVmHostGroup Group { get; }

    public async Task<CommandResult> Create(AzVmHostCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzVmHostDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmHostDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> GetInstanceView(AzVmHostGetInstanceViewOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmHostGetInstanceViewOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzVmHostListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> ListResizeOptions(AzVmHostListResizeOptionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmHostListResizeOptionsOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Resize(AzVmHostResizeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmHostResizeOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Restart(AzVmHostRestartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmHostRestartOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzVmHostShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmHostShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzVmHostUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmHostUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzVmHostWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmHostWaitOptions(), cancellationToken: token);
    }
}