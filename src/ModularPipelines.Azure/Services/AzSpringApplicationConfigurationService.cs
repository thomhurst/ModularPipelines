using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring")]
public class AzSpringApplicationConfigurationService
{
    public AzSpringApplicationConfigurationService(
        AzSpringApplicationConfigurationServiceGit git,
        ICommand internalCommand
    )
    {
        Git = git;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSpringApplicationConfigurationServiceGit Git { get; }

    public async Task<CommandResult> Bind(AzSpringApplicationConfigurationServiceBindOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Clear(AzSpringApplicationConfigurationServiceClearOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzSpringApplicationConfigurationServiceCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSpringApplicationConfigurationServiceDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSpringApplicationConfigurationServiceShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Unbind(AzSpringApplicationConfigurationServiceUnbindOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzSpringApplicationConfigurationServiceUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}