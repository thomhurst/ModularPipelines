using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring-cloud")]
public class AzSpringCloudConfigServer
{
    public AzSpringCloudConfigServer(
        AzSpringCloudConfigServerGit git,
        ICommand internalCommand
    )
    {
        Git = git;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSpringCloudConfigServerGit Git { get; }

    public async Task<CommandResult> Clear(AzSpringCloudConfigServerClearOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Set(AzSpringCloudConfigServerSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSpringCloudConfigServerShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}

