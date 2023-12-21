using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "du")]
public class AzIotDuAccount
{
    public AzIotDuAccount(
        AzIotDuAccountPrivateEndpointConnection privateEndpointConnection,
        AzIotDuAccountPrivateLinkResource privateLinkResource,
        ICommand internalCommand
    )
    {
        PrivateEndpointConnection = privateEndpointConnection;
        PrivateLinkResource = privateLinkResource;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzIotDuAccountPrivateEndpointConnection PrivateEndpointConnection { get; }

    public AzIotDuAccountPrivateLinkResource PrivateLinkResource { get; }

    public async Task<CommandResult> Create(AzIotDuAccountCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzIotDuAccountDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzIotDuAccountListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzIotDuAccountListOptions(), token);
    }

    public async Task<CommandResult> Show(AzIotDuAccountShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzIotDuAccountUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzIotDuAccountWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}