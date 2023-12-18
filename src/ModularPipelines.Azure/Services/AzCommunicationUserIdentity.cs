using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("communication")]
public class AzCommunicationUserIdentity
{
    public AzCommunicationUserIdentity(
        AzCommunicationUserIdentityToken token,
        AzCommunicationUserIdentityUser user,
        ICommand internalCommand
    )
    {
        Token = token;
        User = user;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCommunicationUserIdentityToken Token { get; }

    public AzCommunicationUserIdentityUser User { get; }

    public async Task<CommandResult> IssueAccessToken(AzCommunicationUserIdentityIssueAccessTokenOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}