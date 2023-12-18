using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzVmss
{
    public AzVmss(
        AzVmssApplication application,
        AzVmssDiagnostics diagnostics,
        AzVmssDisk disk,
        AzVmssEncryption encryption,
        AzVmssExtension extension,
        AzVmssIdentity identity,
        AzVmssNic nic,
        AzVmssRollingUpgrade rollingUpgrade,
        AzVmssRunCommand runCommand,
        ICommand internalCommand
    )
    {
        Application = application;
        Diagnostics = diagnostics;
        Disk = disk;
        Encryption = encryption;
        Extension = extension;
        Identity = identity;
        Nic = nic;
        RollingUpgrade = rollingUpgrade;
        RunCommand = runCommand;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzVmssApplication Application { get; }

    public AzVmssDiagnostics Diagnostics { get; }

    public AzVmssDisk Disk { get; }

    public AzVmssEncryption Encryption { get; }

    public AzVmssExtension Extension { get; }

    public AzVmssIdentity Identity { get; }

    public AzVmssNic Nic { get; }

    public AzVmssRollingUpgrade RollingUpgrade { get; }

    public AzVmssRunCommand RunCommand { get; }

    public async Task<CommandResult> Create(AzVmssCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Deallocate(AzVmssDeallocateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzVmssDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteInstances(AzVmssDeleteInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetInstanceView(AzVmssGetInstanceViewOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetOsUpgradeHistory(AzVmssGetOsUpgradeHistoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzVmssListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListInstanceConnectionInfo(AzVmssListInstanceConnectionInfoOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListInstancePublicIps(AzVmssListInstancePublicIpsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListInstances(AzVmssListInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSkus(AzVmssListSkusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PerformMaintenance(AzVmssPerformMaintenanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Reimage(AzVmssReimageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Restart(AzVmssRestartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Scale(AzVmssScaleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetOrchestrationServiceState(AzVmssSetOrchestrationServiceStateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzVmssShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SimulateEviction(AzVmssSimulateEvictionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Start(AzVmssStartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Stop(AzVmssStopOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzVmssUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateInstances(AzVmssUpdateInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzVmssWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmssWaitOptions(), token);
    }
}