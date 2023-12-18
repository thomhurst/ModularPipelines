using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring-cloud")]
public class AzSpringCloudApplicationConfigurationService
{
    public AzSpringCloudApplicationConfigurationService(
        AzSpringCloudApplicationConfigurationServiceGit git,
        ICommand internalCommand
    )
    {
        Git = git;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSpringCloudApplicationConfigurationServiceGit Git { get; }

    public async Task<CommandResult> Bind(AzSpringCloudApplicationConfigurationServiceBindOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Clear(AzSpringCloudApplicationConfigurationServiceClearOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSpringCloudApplicationConfigurationServiceShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Unbind(AzSpringCloudApplicationConfigurationServiceUnbindOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}