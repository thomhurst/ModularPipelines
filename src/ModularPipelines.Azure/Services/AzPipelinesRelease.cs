using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    public async Task<CommandResult> Create(AzPipelinesReleaseCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzPipelinesReleaseListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzPipelinesReleaseShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}