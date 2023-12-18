using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("communication")]
public class AzCommunicationIdentity
{
    public AzCommunicationIdentity(
        AzCommunicationIdentityToken token,
        AzCommunicationIdentityUser user,
        ICommand internalCommand
    )
    {
        Token = token;
        User = user;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCommunicationIdentityToken Token { get; }

    public AzCommunicationIdentityUser User { get; }

    public async Task<CommandResult> Assign(AzCommunicationIdentityAssignOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Remove(AzCommunicationIdentityRemoveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzCommunicationIdentityShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzCommunicationIdentityWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCommunicationIdentityWaitOptions(), token);
    }
}

