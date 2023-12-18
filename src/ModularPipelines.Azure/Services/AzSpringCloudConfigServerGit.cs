using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring-cloud", "config-server")]
public class AzSpringCloudConfigServerGit
{
    public AzSpringCloudConfigServerGit(
        AzSpringCloudConfigServerGitRepo repo,
        ICommand internalCommand
    )
    {
        Repo = repo;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSpringCloudConfigServerGitRepo Repo { get; }

    public async Task<CommandResult> Set(AzSpringCloudConfigServerGitSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}