using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pipelines")]
public class AzPipelinesRelease
{
    public AzPipelinesRelease(
        AzPipelinesReleaseDefinition definition,
        ICommand internalCommand
    )
    {
        Definition = definition;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzPipelinesReleaseDefinition Definition { get; }

    public async Task<CommandResult> Create(AzPipelinesReleaseCreateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPipelinesReleaseCreateOptions(), token);
    }

    public async Task<CommandResult> List(AzPipelinesReleaseListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPipelinesReleaseListOptions(), token);
    }

    public async Task<CommandResult> Show(AzPipelinesReleaseShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}