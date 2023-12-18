using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventhubs")]
public class AzEventhubsNamespace
{
    public AzEventhubsNamespace(
        AzEventhubsNamespaceApplicationGroup applicationGroup,
        AzEventhubsNamespaceAuthorizationRule authorizationRule,
        AzEventhubsNamespaceEncryption encryption,
        AzEventhubsNamespaceIdentity identity,
        AzEventhubsNamespaceNetworkRuleSet networkRuleSet,
        AzEventhubsNamespacePrivateEndpointConnection privateEndpointConnection,
        AzEventhubsNamespacePrivateLinkResource privateLinkResource,
        AzEventhubsNamespaceSchemaRegistry schemaRegistry,
        ICommand internalCommand
    )
    {
        ApplicationGroup = applicationGroup;
        AuthorizationRule = authorizationRule;
        Encryption = encryption;
        Identity = identity;
        NetworkRuleSet = networkRuleSet;
        PrivateEndpointConnection = privateEndpointConnection;
        PrivateLinkResource = privateLinkResource;
        SchemaRegistry = schemaRegistry;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzEventhubsNamespaceApplicationGroup ApplicationGroup { get; }

    public AzEventhubsNamespaceAuthorizationRule AuthorizationRule { get; }

    public AzEventhubsNamespaceEncryption Encryption { get; }

    public AzEventhubsNamespaceIdentity Identity { get; }

    public AzEventhubsNamespaceNetworkRuleSet NetworkRuleSet { get; }

    public AzEventhubsNamespacePrivateEndpointConnection PrivateEndpointConnection { get; }

    public AzEventhubsNamespacePrivateLinkResource PrivateLinkResource { get; }

    public AzEventhubsNamespaceSchemaRegistry SchemaRegistry { get; }

    public async Task<CommandResult> Create(AzEventhubsNamespaceCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzEventhubsNamespaceDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Exists(AzEventhubsNamespaceExistsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzEventhubsNamespaceListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventhubsNamespaceListOptions(), token);
    }

    public async Task<CommandResult> Show(AzEventhubsNamespaceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventhubsNamespaceShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzEventhubsNamespaceUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventhubsNamespaceUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzEventhubsNamespaceWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventhubsNamespaceWaitOptions(), token);
    }
}