using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzSerialConsole
{
    public AzSerialConsole(
        AzSerialConsoleSend send,
        ICommand internalCommand
    )
    {
        Send = send;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSerialConsoleSend Send { get; }

    public async Task<CommandResult> Connect(AzSerialConsoleConnectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Disable(AzSerialConsoleDisableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSerialConsoleDisableOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Enable(AzSerialConsoleEnableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSerialConsoleEnableOptions(), cancellationToken: token);
    }
}