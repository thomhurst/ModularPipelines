using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

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
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Disable(AzSerialConsoleDisableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSerialConsoleDisableOptions(), token);
    }

    public async Task<CommandResult> Enable(AzSerialConsoleEnableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSerialConsoleEnableOptions(), token);
    }
}