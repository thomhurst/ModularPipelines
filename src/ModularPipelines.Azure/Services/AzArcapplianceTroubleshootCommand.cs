using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcappliance", "troubleshoot")]
public class AzArcapplianceTroubleshootCommand
{
    public AzArcapplianceTroubleshootCommand(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Hci(AzArcapplianceTroubleshootCommandHciOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzArcapplianceTroubleshootCommandHciOptions(), token);
    }

    public async Task<CommandResult> Scvmm(AzArcapplianceTroubleshootCommandScvmmOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzArcapplianceTroubleshootCommandScvmmOptions(), token);
    }

    public async Task<CommandResult> Vmware(AzArcapplianceTroubleshootCommandVmwareOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzArcapplianceTroubleshootCommandVmwareOptions(), token);
    }
}