using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring-cloud", "build-service")]
public class AzSpringCloudBuildServiceBuilder
{
    public AzSpringCloudBuildServiceBuilder(
        AzSpringCloudBuildServiceBuilderBuildpackBinding buildpackBinding,
        ICommand internalCommand
    )
    {
        BuildpackBinding = buildpackBinding;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSpringCloudBuildServiceBuilderBuildpackBinding BuildpackBinding { get; }

    public async Task<CommandResult> Create(AzSpringCloudBuildServiceBuilderCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSpringCloudBuildServiceBuilderDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSpringCloudBuildServiceBuilderShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzSpringCloudBuildServiceBuilderUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}