using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("blueprint")]
public class AzBlueprintVersion
{
    public AzBlueprintVersion(
        AzBlueprintVersionArtifact artifact,
        ICommand internalCommand
    )
    {
        Artifact = artifact;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzBlueprintVersionArtifact Artifact { get; }

    public async Task<CommandResult> Delete(AzBlueprintVersionDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzBlueprintVersionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzBlueprintVersionShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}