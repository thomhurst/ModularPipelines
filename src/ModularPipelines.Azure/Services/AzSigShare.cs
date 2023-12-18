using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

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

    public async Task<CommandResult> Add(AzSigShareAddOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSigShareAddOptions(), token);
    }

    public async Task<CommandResult> EnableCommunity(AzSigShareEnableCommunityOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSigShareEnableCommunityOptions(), token);
    }

    public async Task<CommandResult> Remove(AzSigShareRemoveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSigShareRemoveOptions(), token);
    }

    public async Task<CommandResult> Reset(AzSigShareResetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSigShareResetOptions(), token);
    }

    public async Task<CommandResult> Wait(AzSigShareWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}