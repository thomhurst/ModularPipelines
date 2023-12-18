using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dynatrace", "monitor")]
public class AzDynatraceMonitorSsoConfig
{
    public AzDynatraceMonitorSsoConfig(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzDynatraceMonitorSsoConfigCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzDynatraceMonitorSsoConfigListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzDynatraceMonitorSsoConfigShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDynatraceMonitorSsoConfigShowOptions(), token);
    }

    public async Task<CommandResult> Wait(AzDynatraceMonitorSsoConfigWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDynatraceMonitorSsoConfigWaitOptions(), token);
    }
}