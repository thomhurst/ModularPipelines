using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage")]
public class AzStorageAccount
{
    public AzStorageAccount(
        AzStorageAccountBlobInventoryPolicy blobInventoryPolicy,
        AzStorageAccountBlobServiceProperties blobServiceProperties,
        AzStorageAccountCreate create,
        AzStorageAccountEncryptionScope encryptionScope,
        AzStorageAccountFileServiceProperties fileServiceProperties,
        AzStorageAccountHnsMigration hnsMigration,
        AzStorageAccountKeys keys,
        AzStorageAccountLocalUser localUser,
        AzStorageAccountManagementPolicy managementPolicy,
        AzStorageAccountMigration migration,
        AzStorageAccountNetworkRule networkRule,
        AzStorageAccountOrPolicy orPolicy,
        AzStorageAccountPrivateEndpointConnection privateEndpointConnection,
        AzStorageAccountPrivateLinkResource privateLinkResource,
        AzStorageAccountUpdate update,
        ICommand internalCommand
    )
    {
        BlobInventoryPolicy = blobInventoryPolicy;
        BlobServiceProperties = blobServiceProperties;
        CreateCommands = create;
        EncryptionScope = encryptionScope;
        FileServiceProperties = fileServiceProperties;
        HnsMigration = hnsMigration;
        Keys = keys;
        LocalUser = localUser;
        ManagementPolicy = managementPolicy;
        Migration = migration;
        NetworkRule = networkRule;
        OrPolicy = orPolicy;
        PrivateEndpointConnection = privateEndpointConnection;
        PrivateLinkResource = privateLinkResource;
        UpdateCommands = update;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzStorageAccountBlobInventoryPolicy BlobInventoryPolicy { get; }

    public AzStorageAccountBlobServiceProperties BlobServiceProperties { get; }

    public AzStorageAccountCreate CreateCommands { get; }

    public AzStorageAccountEncryptionScope EncryptionScope { get; }

    public AzStorageAccountFileServiceProperties FileServiceProperties { get; }

    public AzStorageAccountHnsMigration HnsMigration { get; }

    public AzStorageAccountKeys Keys { get; }

    public AzStorageAccountLocalUser LocalUser { get; }

    public AzStorageAccountManagementPolicy ManagementPolicy { get; }

    public AzStorageAccountMigration Migration { get; }

    public AzStorageAccountNetworkRule NetworkRule { get; }

    public AzStorageAccountOrPolicy OrPolicy { get; }

    public AzStorageAccountPrivateEndpointConnection PrivateEndpointConnection { get; }

    public AzStorageAccountPrivateLinkResource PrivateLinkResource { get; }

    public AzStorageAccountUpdate UpdateCommands { get; }

    public async Task<CommandResult> CheckName(AzStorageAccountCheckNameOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzStorageAccountCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzStorageAccountDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageAccountDeleteOptions(), token);
    }

    public async Task<CommandResult> Failover(AzStorageAccountFailoverOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageAccountFailoverOptions(), token);
    }

    public async Task<CommandResult> GenerateSas(AzStorageAccountGenerateSasOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzStorageAccountListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageAccountListOptions(), token);
    }

    public async Task<CommandResult> RevokeDelegationKeys(AzStorageAccountRevokeDelegationKeysOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageAccountRevokeDelegationKeysOptions(), token);
    }

    public async Task<CommandResult> Show(AzStorageAccountShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageAccountShowOptions(), token);
    }

    public async Task<CommandResult> ShowConnectionString(AzStorageAccountShowConnectionStringOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageAccountShowConnectionStringOptions(), token);
    }

    public async Task<CommandResult> ShowUsage(AzStorageAccountShowUsageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzStorageAccountUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageAccountUpdateOptions(), token);
    }
}