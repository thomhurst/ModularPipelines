using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("sphere", "device")]
public class AzSphereDeviceApp
{
    public AzSphereDeviceApp(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> ShowMemoryStats(AzSphereDeviceAppShowMemoryStatsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSphereDeviceAppShowMemoryStatsOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ShowQuota(AzSphereDeviceAppShowQuotaOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSphereDeviceAppShowQuotaOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ShowStatus(AzSphereDeviceAppShowStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSphereDeviceAppShowStatusOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Start(AzSphereDeviceAppStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSphereDeviceAppStartOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Stop(AzSphereDeviceAppStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSphereDeviceAppStopOptions(), cancellationToken: token);
    }
}