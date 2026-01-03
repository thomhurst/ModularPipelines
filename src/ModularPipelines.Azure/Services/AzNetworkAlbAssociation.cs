using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("network", "alb")]
public class AzNetworkAlbAssociation
{
    public AzNetworkAlbAssociation(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkAlbAssociationCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzNetworkAlbAssociationDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkAlbAssociationDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzNetworkAlbAssociationListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzNetworkAlbAssociationShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkAlbAssociationShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzNetworkAlbAssociationUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkAlbAssociationUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzNetworkAlbAssociationWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkAlbAssociationWaitOptions(), cancellationToken: token);
    }
}