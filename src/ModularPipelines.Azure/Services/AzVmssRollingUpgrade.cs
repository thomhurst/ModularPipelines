using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmss")]
public class AzVmssRollingUpgrade
{
    public AzVmssRollingUpgrade(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Cancel(AzVmssRollingUpgradeCancelOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmssRollingUpgradeCancelOptions(), token);
    }

    public async Task<CommandResult> GetLatest(AzVmssRollingUpgradeGetLatestOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmssRollingUpgradeGetLatestOptions(), token);
    }

    public async Task<CommandResult> Start(AzVmssRollingUpgradeStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmssRollingUpgradeStartOptions(), token);
    }
}