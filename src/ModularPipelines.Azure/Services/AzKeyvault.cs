using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzKeyvault
{
    public AzKeyvault(
        AzKeyvaultBackup backup,
        AzKeyvaultCertificate certificate,
        AzKeyvaultKey key,
        AzKeyvaultNetworkRule networkRule,
        AzKeyvaultPrivateEndpointConnection privateEndpointConnection,
        AzKeyvaultPrivateLinkResource privateLinkResource,
        AzKeyvaultRegion region,
        AzKeyvaultRestore restore,
        AzKeyvaultRole role,
        AzKeyvaultSecret secret,
        AzKeyvaultSecurityDomain securityDomain,
        AzKeyvaultSetting setting,
        ICommand internalCommand
    )
    {
        Backup = backup;
        Certificate = certificate;
        Key = key;
        NetworkRule = networkRule;
        PrivateEndpointConnection = privateEndpointConnection;
        PrivateLinkResource = privateLinkResource;
        Region = region;
        Restore = restore;
        Role = role;
        Secret = secret;
        SecurityDomain = securityDomain;
        Setting = setting;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzKeyvaultBackup Backup { get; }

    public AzKeyvaultCertificate Certificate { get; }

    public AzKeyvaultKey Key { get; }

    public AzKeyvaultNetworkRule NetworkRule { get; }

    public AzKeyvaultPrivateEndpointConnection PrivateEndpointConnection { get; }

    public AzKeyvaultPrivateLinkResource PrivateLinkResource { get; }

    public AzKeyvaultRegion Region { get; }

    public AzKeyvaultRestore Restore { get; }

    public AzKeyvaultRole Role { get; }

    public AzKeyvaultSecret Secret { get; }

    public AzKeyvaultSecurityDomain SecurityDomain { get; }

    public AzKeyvaultSetting Setting { get; }

    public async Task<CommandResult> CheckName(AzKeyvaultCheckNameOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzKeyvaultCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzKeyvaultDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultDeleteOptions(), token);
    }

    public async Task<CommandResult> DeletePolicy(AzKeyvaultDeletePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzKeyvaultListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultListOptions(), token);
    }

    public async Task<CommandResult> ListDeleted(AzKeyvaultListDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultListDeletedOptions(), token);
    }

    public async Task<CommandResult> Purge(AzKeyvaultPurgeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultPurgeOptions(), token);
    }

    public async Task<CommandResult> Recover(AzKeyvaultRecoverOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultRecoverOptions(), token);
    }

    public async Task<CommandResult> SetPolicy(AzKeyvaultSetPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzKeyvaultShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultShowOptions(), token);
    }

    public async Task<CommandResult> ShowDeleted(AzKeyvaultShowDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultShowDeletedOptions(), token);
    }

    public async Task<CommandResult> Update(AzKeyvaultUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateHsm(AzKeyvaultUpdateHsmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzKeyvaultWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> WaitHsm(AzKeyvaultWaitHsmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}