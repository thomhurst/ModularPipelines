using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm")]
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
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzVmHostDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetInstanceView(AzVmHostGetInstanceViewOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzVmHostListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListResizeOptions(AzVmHostListResizeOptionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmHostListResizeOptionsOptions(), token);
    }

    public async Task<CommandResult> Resize(AzVmHostResizeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmHostResizeOptions(), token);
    }

    public async Task<CommandResult> Restart(AzVmHostRestartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmHostRestartOptions(), token);
    }

    public async Task<CommandResult> Show(AzVmHostShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmHostShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzVmHostUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmHostUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzVmHostWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmHostWaitOptions(), token);
    }
}

