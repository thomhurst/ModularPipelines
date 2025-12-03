using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("blueprint")]
public class AzBlueprintArtifact
{
    public AzBlueprintArtifact(
        AzBlueprintArtifactPolicy policy,
        AzBlueprintArtifactRole role,
        AzBlueprintArtifactTemplate template,
        ICommand internalCommand
    )
    {
        Policy = policy;
        Role = role;
        Template = template;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzBlueprintArtifactPolicy Policy { get; }

    public AzBlueprintArtifactRole Role { get; }

    public AzBlueprintArtifactTemplate Template { get; }

    public async Task<CommandResult> Delete(AzBlueprintArtifactDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzBlueprintArtifactListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzBlueprintArtifactShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}