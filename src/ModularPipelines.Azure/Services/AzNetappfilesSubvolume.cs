using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles")]
public class AzNetappfilesSubvolume
{
    public AzNetappfilesSubvolume(
        AzNetappfilesSubvolumeMetadata metadata,
        ICommand internalCommand
    )
    {
        Metadata = metadata;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetappfilesSubvolumeMetadata Metadata { get; }

    public async Task<CommandResult> Create(AzNetappfilesSubvolumeCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetappfilesSubvolumeDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetappfilesSubvolumeListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetappfilesSubvolumeShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesSubvolumeShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetappfilesSubvolumeUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesSubvolumeUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetappfilesSubvolumeWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesSubvolumeWaitOptions(), token);
    }
}

