using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("databoxedge")]
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
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDataboxedgeDeviceDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataboxedgeDeviceDeleteOptions(), token);
    }

    public async Task<CommandResult> DownloadUpdate(AzDataboxedgeDeviceDownloadUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataboxedgeDeviceDownloadUpdateOptions(), token);
    }

    public async Task<CommandResult> InstallUpdate(AzDataboxedgeDeviceInstallUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataboxedgeDeviceInstallUpdateOptions(), token);
    }

    public async Task<CommandResult> List(AzDataboxedgeDeviceListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataboxedgeDeviceListOptions(), token);
    }

    public async Task<CommandResult> ScanForUpdate(AzDataboxedgeDeviceScanForUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataboxedgeDeviceScanForUpdateOptions(), token);
    }

    public async Task<CommandResult> Show(AzDataboxedgeDeviceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataboxedgeDeviceShowOptions(), token);
    }

    public async Task<CommandResult> ShowUpdateSummary(AzDataboxedgeDeviceShowUpdateSummaryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataboxedgeDeviceShowUpdateSummaryOptions(), token);
    }

    public async Task<CommandResult> Update(AzDataboxedgeDeviceUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataboxedgeDeviceUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzDataboxedgeDeviceWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataboxedgeDeviceWaitOptions(), token);
    }
}