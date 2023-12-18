using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzBlueprint
{
    public AzBlueprint(
        AzBlueprintArtifact artifact,
        AzBlueprintAssignment assignment,
        AzBlueprintResourceGroup resourceGroup,
        AzBlueprintVersion version,
        ICommand internalCommand
    )
    {
        Artifact = artifact;
        Assignment = assignment;
        ResourceGroup = resourceGroup;
        Version = version;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzBlueprintArtifact Artifact { get; }

    public AzBlueprintAssignment Assignment { get; }

    public AzBlueprintResourceGroup ResourceGroup { get; }

    public AzBlueprintVersion Version { get; }

    public async Task<CommandResult> Create(AzBlueprintCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzBlueprintDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Export(AzBlueprintExportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Import(AzBlueprintImportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzBlueprintListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Publish(AzBlueprintPublishOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzBlueprintShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzBlueprintUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}