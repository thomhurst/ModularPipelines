using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicebus")]
public class AzServicebusNamespace
{
    public AzServicebusNamespace(
        AzServicebusNamespaceAuthorizationRule authorizationRule,
        AzServicebusNamespaceEncryption encryption,
        AzServicebusNamespaceIdentity identity,
        AzServicebusNamespaceNetworkRuleSet networkRuleSet,
        AzServicebusNamespacePrivateEndpointConnection privateEndpointConnection,
        AzServicebusNamespacePrivateLinkResource privateLinkResource,
        ICommand internalCommand
    )
    {
        AuthorizationRule = authorizationRule;
        Encryption = encryption;
        Identity = identity;
        NetworkRuleSet = networkRuleSet;
        PrivateEndpointConnection = privateEndpointConnection;
        PrivateLinkResource = privateLinkResource;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzServicebusNamespaceAuthorizationRule AuthorizationRule { get; }

    public AzServicebusNamespaceEncryption Encryption { get; }

    public AzServicebusNamespaceIdentity Identity { get; }

    public AzServicebusNamespaceNetworkRuleSet NetworkRuleSet { get; }

    public AzServicebusNamespacePrivateEndpointConnection PrivateEndpointConnection { get; }

    public AzServicebusNamespacePrivateLinkResource PrivateLinkResource { get; }

    public async Task<CommandResult> Create(AzServicebusNamespaceCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzServicebusNamespaceDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Exists(AzServicebusNamespaceExistsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzServicebusNamespaceListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusNamespaceListOptions(), token);
    }

    public async Task<CommandResult> Show(AzServicebusNamespaceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusNamespaceShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzServicebusNamespaceUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusNamespaceUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzServicebusNamespaceWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusNamespaceWaitOptions(), token);
    }
}

