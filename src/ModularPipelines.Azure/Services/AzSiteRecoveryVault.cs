using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("site-recovery")]
public class AzSiteRecoveryVault
{
    public AzSiteRecoveryVault(
        AzSiteRecoveryVaultHealth health,
        ICommand internalCommand
    )
    {
        Health = health;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSiteRecoveryVaultHealth Health { get; }

    public async Task<CommandResult> ListAppliance(AzSiteRecoveryVaultListApplianceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListMigrationItem(AzSiteRecoveryVaultListMigrationItemOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListNetwork(AzSiteRecoveryVaultListNetworkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListNetworkMapping(AzSiteRecoveryVaultListNetworkMappingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListProtectedItem(AzSiteRecoveryVaultListProtectedItemOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListProtectionContainer(AzSiteRecoveryVaultListProtectionContainerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListProtectionContainerMapping(AzSiteRecoveryVaultListProtectionContainerMappingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListRecoveryServicesProvider(AzSiteRecoveryVaultListRecoveryServicesProviderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListStorageClassification(AzSiteRecoveryVaultListStorageClassificationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListStorageClassificationMapping(AzSiteRecoveryVaultListStorageClassificationMappingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListVCenter(AzSiteRecoveryVaultListVCenterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ShowSupportedOperatingSystem(AzSiteRecoveryVaultShowSupportedOperatingSystemOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSiteRecoveryVaultShowSupportedOperatingSystemOptions(), token);
    }
}

