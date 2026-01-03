using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("databoxedge")]
public class AzDataboxedgeDevice
{
    public AzDataboxedgeDevice(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzDataboxedgeDeviceCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzDataboxedgeDeviceDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataboxedgeDeviceDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> DownloadUpdate(AzDataboxedgeDeviceDownloadUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataboxedgeDeviceDownloadUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> InstallUpdate(AzDataboxedgeDeviceInstallUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataboxedgeDeviceInstallUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzDataboxedgeDeviceListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataboxedgeDeviceListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ScanForUpdate(AzDataboxedgeDeviceScanForUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataboxedgeDeviceScanForUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzDataboxedgeDeviceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataboxedgeDeviceShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ShowUpdateSummary(AzDataboxedgeDeviceShowUpdateSummaryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataboxedgeDeviceShowUpdateSummaryOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzDataboxedgeDeviceUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataboxedgeDeviceUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzDataboxedgeDeviceWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataboxedgeDeviceWaitOptions(), cancellationToken: token);
    }
}