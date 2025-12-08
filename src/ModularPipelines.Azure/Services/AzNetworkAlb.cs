using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("network")]
public class AzNetworkAlb
{
    public AzNetworkAlb(
        AzNetworkAlbAssociation association,
        AzNetworkAlbFrontend frontend,
        ICommand internalCommand
    )
    {
        Association = association;
        Frontend = frontend;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkAlbAssociation Association { get; }

    public AzNetworkAlbFrontend Frontend { get; }

    public async Task<CommandResult> Create(AzNetworkAlbCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkAlbDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkAlbDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkAlbListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkAlbListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkAlbShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkAlbShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkAlbUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkAlbUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkAlbWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkAlbWaitOptions(), token);
    }
}