using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("netappfiles")]
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

    public async Task<CommandResult> Delete(AzNetappfilesSubvolumeDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesSubvolumeDeleteOptions(), token);
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