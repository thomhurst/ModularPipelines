using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("remote-rendering-account")]
public class AzRemoteRenderingAccountKey
{
    public AzRemoteRenderingAccountKey(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Renew(AzRemoteRenderingAccountKeyRenewOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzRemoteRenderingAccountKeyRenewOptions(), token);
    }

    public async Task<CommandResult> Show(AzRemoteRenderingAccountKeyShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzRemoteRenderingAccountKeyShowOptions(), token);
    }
}

