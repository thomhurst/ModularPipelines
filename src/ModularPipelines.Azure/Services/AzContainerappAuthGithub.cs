using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "auth")]
public class AzContainerappAuthGithub
{
    public AzContainerappAuthGithub(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Show(AzContainerappAuthGithubShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappAuthGithubShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzContainerappAuthGithubUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappAuthGithubUpdateOptions(), token);
    }
}