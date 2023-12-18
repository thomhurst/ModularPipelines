using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central")]
public class AzIotCentralDevice
{
    public AzIotCentralDevice(
        AzIotCentralDeviceAttestation attestation,
        AzIotCentralDeviceC2dMessage c2dMessage,
        AzIotCentralDeviceCommand command,
        AzIotCentralDeviceEdge edge,
        AzIotCentralDeviceTelemetry telemetry,
        AzIotCentralDeviceTwin twin,
        ICommand internalCommand
    )
    {
        Attestation = attestation;
        C2dMessage = c2dMessage;
        Command = command;
        Edge = edge;
        Telemetry = telemetry;
        Twin = twin;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzIotCentralDeviceAttestation Attestation { get; }

    public AzIotCentralDeviceC2dMessage C2dMessage { get; }

    public AzIotCentralDeviceCommand Command { get; }

    public AzIotCentralDeviceEdge Edge { get; }

    public AzIotCentralDeviceTelemetry Telemetry { get; }

    public AzIotCentralDeviceTwin Twin { get; }

    public async Task<CommandResult> ComputeDeviceKey(AzIotCentralDeviceComputeDeviceKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzIotCentralDeviceCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzIotCentralDeviceDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzIotCentralDeviceListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListComponents(AzIotCentralDeviceListComponentsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListModules(AzIotCentralDeviceListModulesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ManualFailback(AzIotCentralDeviceManualFailbackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ManualFailover(AzIotCentralDeviceManualFailoverOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegistrationInfo(AzIotCentralDeviceRegistrationInfoOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzIotCentralDeviceShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ShowCredentials(AzIotCentralDeviceShowCredentialsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzIotCentralDeviceUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}