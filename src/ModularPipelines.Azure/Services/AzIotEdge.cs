using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot")]
public class AzIotEdge
{
    public AzIotEdge(
        AzIotEdgeDeployment deployment,
        AzIotEdgeDevices devices,
        ICommand internalCommand
    )
    {
        Deployment = deployment;
        Devices = devices;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzIotEdgeDeployment Deployment { get; }

    public AzIotEdgeDevices Devices { get; }

    public async Task<CommandResult> ExportModules(AzIotEdgeExportModulesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetModules(AzIotEdgeSetModulesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}