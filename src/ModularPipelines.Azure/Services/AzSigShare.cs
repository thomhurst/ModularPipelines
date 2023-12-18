using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sig")]
public class AzSigShare
{
    public AzSigShare(
        AzSigShareEnableCommunity enableCommunity,
        ICommand internalCommand
    )
    {
        EnableCommunityCommands = enableCommunity;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSigShareEnableCommunity EnableCommunityCommands { get; }

    public async Task<CommandResult> Add(AzSigShareAddOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableCommunity(AzSigShareEnableCommunityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Remove(AzSigShareRemoveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Reset(AzSigShareResetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzSigShareWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}

