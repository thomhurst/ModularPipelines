using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "build-service")]
public class AzSpringBuildServiceBuild
{
    public AzSpringBuildServiceBuild(
        AzSpringBuildServiceBuildResult result,
        ICommand internalCommand
    )
    {
        Result = result;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSpringBuildServiceBuildResult Result { get; }

    public async Task<CommandResult> Create(AzSpringBuildServiceBuildCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSpringBuildServiceBuildDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzSpringBuildServiceBuildListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSpringBuildServiceBuildShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzSpringBuildServiceBuildUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}