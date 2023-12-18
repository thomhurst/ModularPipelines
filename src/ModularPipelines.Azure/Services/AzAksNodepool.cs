using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks")]
public class AzAksNodepool
{
    public AzAksNodepool(
        AzAksNodepoolAdd add,
        AzAksNodepoolDelete delete,
        AzAksNodepoolGetUpgrades getUpgrades,
        AzAksNodepoolList list,
        AzAksNodepoolOperationAbort operationAbort,
        AzAksNodepoolScale scale,
        AzAksNodepoolShow show,
        AzAksNodepoolSnapshot snapshot,
        AzAksNodepoolStart start,
        AzAksNodepoolStop stop,
        AzAksNodepoolUpdate update,
        AzAksNodepoolUpgrade upgrade,
        ICommand internalCommand
    )
    {
        AddCommands = add;
        DeleteCommands = delete;
        GetUpgradesCommands = getUpgrades;
        ListCommands = list;
        OperationAbortCommands = operationAbort;
        ScaleCommands = scale;
        ShowCommands = show;
        Snapshot = snapshot;
        StartCommands = start;
        StopCommands = stop;
        UpdateCommands = update;
        UpgradeCommands = upgrade;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAksNodepoolAdd AddCommands { get; }

    public AzAksNodepoolDelete DeleteCommands { get; }

    public AzAksNodepoolGetUpgrades GetUpgradesCommands { get; }

    public AzAksNodepoolList ListCommands { get; }

    public AzAksNodepoolOperationAbort OperationAbortCommands { get; }

    public AzAksNodepoolScale ScaleCommands { get; }

    public AzAksNodepoolShow ShowCommands { get; }

    public AzAksNodepoolSnapshot Snapshot { get; }

    public AzAksNodepoolStart StartCommands { get; }

    public AzAksNodepoolStop StopCommands { get; }

    public AzAksNodepoolUpdate UpdateCommands { get; }

    public AzAksNodepoolUpgrade UpgradeCommands { get; }

    public async Task<CommandResult> Add(AzAksNodepoolAddOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAksNodepoolDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetUpgrades(AzAksNodepoolGetUpgradesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzAksNodepoolListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> OperationAbort(AzAksNodepoolOperationAbortOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Scale(AzAksNodepoolScaleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzAksNodepoolShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Start(AzAksNodepoolStartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Stop(AzAksNodepoolStopOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzAksNodepoolUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Upgrade(AzAksNodepoolUpgradeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzAksNodepoolWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}