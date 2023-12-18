using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "du")]
public class AzIotDuDevice
{
    public AzIotDuDevice(
        AzIotDuDeviceClass @class,
        AzIotDuDeviceCompliance compliance,
        AzIotDuDeviceDeployment deployment,
        AzIotDuDeviceGroup group,
        AzIotDuDeviceHealth health,
        AzIotDuDeviceLog log,
        AzIotDuDeviceModule module,
        ICommand internalCommand
    )
    {
        Class = @class;
        Compliance = compliance;
        Deployment = deployment;
        Group = group;
        Health = health;
        Log = log;
        Module = module;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzIotDuDeviceClass Class { get; }

    public AzIotDuDeviceCompliance Compliance { get; }

    public AzIotDuDeviceDeployment Deployment { get; }

    public AzIotDuDeviceGroup Group { get; }

    public AzIotDuDeviceHealth Health { get; }

    public AzIotDuDeviceLog Log { get; }

    public AzIotDuDeviceModule Module { get; }

    public async Task<CommandResult> Import(AzIotDuDeviceImportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzIotDuDeviceListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzIotDuDeviceShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}