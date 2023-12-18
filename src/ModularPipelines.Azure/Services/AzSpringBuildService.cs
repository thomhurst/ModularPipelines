using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring")]
public class AzSpringBuildService
{
    public AzSpringBuildService(
        AzSpringBuildServiceBuild build,
        AzSpringBuildServiceBuilder builder,
        ICommand internalCommand
    )
    {
        Build = build;
        Builder = builder;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSpringBuildServiceBuild Build { get; }

    public AzSpringBuildServiceBuilder Builder { get; }

    public async Task<CommandResult> Show(AzSpringBuildServiceShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzSpringBuildServiceUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}

