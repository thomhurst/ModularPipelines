using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central")]
public class AzIotCentralApp
{
    public AzIotCentralApp(
        AzIotCentralAppIdentity identity,
        AzIotCentralAppPrivateEndpointConnection privateEndpointConnection,
        AzIotCentralAppPrivateLinkResource privateLinkResource,
        ICommand internalCommand
    )
    {
        Identity = identity;
        PrivateEndpointConnection = privateEndpointConnection;
        PrivateLinkResource = privateLinkResource;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzIotCentralAppIdentity Identity { get; }

    public AzIotCentralAppPrivateEndpointConnection PrivateEndpointConnection { get; }

    public AzIotCentralAppPrivateLinkResource PrivateLinkResource { get; }

    public async Task<CommandResult> Create(AzIotCentralAppCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzIotCentralAppDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzIotCentralAppListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzIotCentralAppListOptions(), token);
    }

    public async Task<CommandResult> Show(AzIotCentralAppShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzIotCentralAppUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}