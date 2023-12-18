using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor")]
public class AzMonitorActionGroup
{
    public AzMonitorActionGroup(
        AzMonitorActionGroupTestNotifications testNotifications,
        ICommand internalCommand
    )
    {
        TestNotifications = testNotifications;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzMonitorActionGroupTestNotifications TestNotifications { get; }

    public async Task<CommandResult> Create(AzMonitorActionGroupCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzMonitorActionGroupDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableReceiver(AzMonitorActionGroupEnableReceiverOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzMonitorActionGroupListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorActionGroupListOptions(), token);
    }

    public async Task<CommandResult> Show(AzMonitorActionGroupShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorActionGroupShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzMonitorActionGroupUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorActionGroupUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzMonitorActionGroupWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorActionGroupWaitOptions(), token);
    }
}

