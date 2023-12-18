using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "build-service")]
public class AzSpringBuildServiceBuilder
{
    public AzSpringBuildServiceBuilder(
        AzSpringBuildServiceBuilderBuildpackBinding buildpackBinding,
        ICommand internalCommand
    )
    {
        BuildpackBinding = buildpackBinding;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSpringBuildServiceBuilderBuildpackBinding BuildpackBinding { get; }

    public async Task<CommandResult> Create(AzSpringBuildServiceBuilderCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSpringBuildServiceBuilderDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSpringBuildServiceBuilderShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ShowDeployments(AzSpringBuildServiceBuilderShowDeploymentsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzSpringBuildServiceBuilderUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}