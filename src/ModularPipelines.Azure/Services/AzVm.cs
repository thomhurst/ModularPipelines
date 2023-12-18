using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzVm
{
    public AzVm(
        AzVmAem aem,
        AzVmApplication application,
        AzVmAvailabilitySet availabilitySet,
        AzVmBootDiagnostics bootDiagnostics,
        AzVmDiagnostics diagnostics,
        AzVmDisk disk,
        AzVmEncryption encryption,
        AzVmExtension extension,
        AzVmHost host,
        AzVmIdentity identity,
        AzVmImage image,
        AzVmMonitor monitor,
        AzVmNic nic,
        AzVmRepair repair,
        AzVmRunCommand runCommand,
        AzVmSecret secret,
        AzVmUnmanagedDisk unmanagedDisk,
        AzVmUser user,
        ICommand internalCommand
    )
    {
        Aem = aem;
        Application = application;
        AvailabilitySet = availabilitySet;
        BootDiagnostics = bootDiagnostics;
        Diagnostics = diagnostics;
        Disk = disk;
        Encryption = encryption;
        Extension = extension;
        Host = host;
        Identity = identity;
        Image = image;
        Monitor = monitor;
        Nic = nic;
        Repair = repair;
        RunCommand = runCommand;
        Secret = secret;
        UnmanagedDisk = unmanagedDisk;
        User = user;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzVmAem Aem { get; }

    public AzVmApplication Application { get; }

    public AzVmAvailabilitySet AvailabilitySet { get; }

    public AzVmBootDiagnostics BootDiagnostics { get; }

    public AzVmDiagnostics Diagnostics { get; }

    public AzVmDisk Disk { get; }

    public AzVmEncryption Encryption { get; }

    public AzVmExtension Extension { get; }

    public AzVmHost Host { get; }

    public AzVmIdentity Identity { get; }

    public AzVmImage Image { get; }

    public AzVmMonitor Monitor { get; }

    public AzVmNic Nic { get; }

    public AzVmRepair Repair { get; }

    public AzVmRunCommand RunCommand { get; }

    public AzVmSecret Secret { get; }

    public AzVmUnmanagedDisk UnmanagedDisk { get; }

    public AzVmUser User { get; }

    public async Task<CommandResult> AssessPatches(AzVmAssessPatchesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmAssessPatchesOptions(), token);
    }

    public async Task<CommandResult> AutoShutdown(AzVmAutoShutdownOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmAutoShutdownOptions(), token);
    }

    public async Task<CommandResult> Capture(AzVmCaptureOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Convert(AzVmConvertOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmConvertOptions(), token);
    }

    public async Task<CommandResult> Create(AzVmCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Deallocate(AzVmDeallocateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmDeallocateOptions(), token);
    }

    public async Task<CommandResult> Delete(AzVmDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmDeleteOptions(), token);
    }

    public async Task<CommandResult> Generalize(AzVmGeneralizeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmGeneralizeOptions(), token);
    }

    public async Task<CommandResult> GetInstanceView(AzVmGetInstanceViewOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmGetInstanceViewOptions(), token);
    }

    public async Task<CommandResult> InstallPatches(AzVmInstallPatchesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzVmListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmListOptions(), token);
    }

    public async Task<CommandResult> ListIpAddresses(AzVmListIpAddressesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmListIpAddressesOptions(), token);
    }

    public async Task<CommandResult> ListSizes(AzVmListSizesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmListSizesOptions(), token);
    }

    public async Task<CommandResult> ListSkus(AzVmListSkusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmListSkusOptions(), token);
    }

    public async Task<CommandResult> ListUsage(AzVmListUsageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListVmResizeOptions(AzVmListVmResizeOptionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmListVmResizeOptionsOptions(), token);
    }

    public async Task<CommandResult> OpenPort(AzVmOpenPortOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PerformMaintenance(AzVmPerformMaintenanceOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmPerformMaintenanceOptions(), token);
    }

    public async Task<CommandResult> Reapply(AzVmReapplyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmReapplyOptions(), token);
    }

    public async Task<CommandResult> Redeploy(AzVmRedeployOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmRedeployOptions(), token);
    }

    public async Task<CommandResult> Reimage(AzVmReimageOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmReimageOptions(), token);
    }

    public async Task<CommandResult> Resize(AzVmResizeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Restart(AzVmRestartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmRestartOptions(), token);
    }

    public async Task<CommandResult> Show(AzVmShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmShowOptions(), token);
    }

    public async Task<CommandResult> SimulateEviction(AzVmSimulateEvictionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmSimulateEvictionOptions(), token);
    }

    public async Task<CommandResult> Start(AzVmStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmStartOptions(), token);
    }

    public async Task<CommandResult> Stop(AzVmStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmStopOptions(), token);
    }

    public async Task<CommandResult> Update(AzVmUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzVmWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmWaitOptions(), token);
    }
}