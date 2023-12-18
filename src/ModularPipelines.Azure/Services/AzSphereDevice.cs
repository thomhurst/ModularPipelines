using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere")]
public class AzSphereDevice
{
    public AzSphereDevice(
        AzSphereDeviceApp app,
        AzSphereDeviceCapability capability,
        AzSphereDeviceCertificate certificate,
        AzSphereDeviceImage image,
        AzSphereDeviceManufacturingState manufacturingState,
        AzSphereDeviceNetwork network,
        AzSphereDeviceSideload sideload,
        AzSphereDeviceWifi wifi,
        ICommand internalCommand
    )
    {
        App = app;
        Capability = capability;
        Certificate = certificate;
        Image = image;
        ManufacturingState = manufacturingState;
        Network = network;
        Sideload = sideload;
        Wifi = wifi;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSphereDeviceApp App { get; }

    public AzSphereDeviceCapability Capability { get; }

    public AzSphereDeviceCertificate Certificate { get; }

    public AzSphereDeviceImage Image { get; }

    public AzSphereDeviceManufacturingState ManufacturingState { get; }

    public AzSphereDeviceNetwork Network { get; }

    public AzSphereDeviceSideload Sideload { get; }

    public AzSphereDeviceWifi Wifi { get; }

    public async Task<CommandResult> Assign(AzSphereDeviceAssignOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Claim(AzSphereDeviceClaimOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableCloudTest(AzSphereDeviceEnableCloudTestOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableDevelopment(AzSphereDeviceEnableDevelopmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzSphereDeviceListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAttached(AzSphereDeviceListAttachedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSphereDeviceListAttachedOptions(), token);
    }

    public async Task<CommandResult> Recover(AzSphereDeviceRecoverOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSphereDeviceRecoverOptions(), token);
    }

    public async Task<CommandResult> RescanAttached(AzSphereDeviceRescanAttachedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSphereDeviceRescanAttachedOptions(), token);
    }

    public async Task<CommandResult> Restart(AzSphereDeviceRestartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSphereDeviceRestartOptions(), token);
    }

    public async Task<CommandResult> Show(AzSphereDeviceShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ShowAttached(AzSphereDeviceShowAttachedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSphereDeviceShowAttachedOptions(), token);
    }

    public async Task<CommandResult> ShowCount(AzSphereDeviceShowCountOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ShowDeploymentStatus(AzSphereDeviceShowDeploymentStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ShowOsVersion(AzSphereDeviceShowOsVersionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSphereDeviceShowOsVersionOptions(), token);
    }

    public async Task<CommandResult> Unassign(AzSphereDeviceUnassignOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}