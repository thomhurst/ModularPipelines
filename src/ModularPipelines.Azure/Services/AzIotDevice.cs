using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot")]
public class AzIotDevice
{
    public AzIotDevice(
        AzIotDeviceC2dMessage c2dMessage,
        AzIotDeviceRegistration registration,
        ICommand internalCommand
    )
    {
        C2dMessage = c2dMessage;
        Registration = registration;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzIotDeviceC2dMessage C2dMessage { get; }

    public AzIotDeviceRegistration Registration { get; }

    public async Task<CommandResult> SendD2cMessage(AzIotDeviceSendD2cMessageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Simulate(AzIotDeviceSimulateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UploadFile(AzIotDeviceUploadFileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}