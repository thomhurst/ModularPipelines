using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkcloud")]
public class AzNetworkcloudStorageappliance
{
    public AzNetworkcloudStorageappliance(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> DisableRemoteVendorManagement(AzNetworkcloudStorageapplianceDisableRemoteVendorManagementOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudStorageapplianceDisableRemoteVendorManagementOptions(), token);
    }

    public async Task<CommandResult> EnableRemoteVendorManagement(AzNetworkcloudStorageapplianceEnableRemoteVendorManagementOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudStorageapplianceEnableRemoteVendorManagementOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkcloudStorageapplianceListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudStorageapplianceListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkcloudStorageapplianceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudStorageapplianceShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkcloudStorageapplianceUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudStorageapplianceUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkcloudStorageapplianceWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudStorageapplianceWaitOptions(), token);
    }
}