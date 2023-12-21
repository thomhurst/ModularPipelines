using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "perimeter")]
public class AzNetworkPerimeterAssociation
{
    public AzNetworkPerimeterAssociation(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkPerimeterAssociationCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkPerimeterAssociationDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPerimeterAssociationDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkPerimeterAssociationListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkPerimeterAssociationShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPerimeterAssociationShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkPerimeterAssociationUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPerimeterAssociationUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkPerimeterAssociationWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPerimeterAssociationWaitOptions(), token);
    }
}