using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcappliance")]
public class AzArcapplianceLogs
{
    public AzArcapplianceLogs(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Hci(AzArcapplianceLogsHciOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzArcapplianceLogsHciOptions(), token);
    }

    public async Task<CommandResult> Scvmm(AzArcapplianceLogsScvmmOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzArcapplianceLogsScvmmOptions(), token);
    }

    public async Task<CommandResult> Vmware(AzArcapplianceLogsVmwareOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzArcapplianceLogsVmwareOptions(), token);
    }
}