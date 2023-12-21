using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "config-server")]
public class AzSpringConfigServerGit
{
    public AzSpringConfigServerGit(
        AzSpringConfigServerGitRepo repo,
        ICommand internalCommand
    )
    {
        Repo = repo;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSpringConfigServerGitRepo Repo { get; }

    public async Task<CommandResult> Set(AzSpringConfigServerGitSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}