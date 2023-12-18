using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "manifest")]
public class AzAcrManifestMetadata
{
    public AzAcrManifestMetadata(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> List(AzAcrManifestMetadataListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAcrManifestMetadataListOptions(), token);
    }

    public async Task<CommandResult> Show(AzAcrManifestMetadataShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAcrManifestMetadataShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzAcrManifestMetadataUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAcrManifestMetadataUpdateOptions(), token);
    }
}