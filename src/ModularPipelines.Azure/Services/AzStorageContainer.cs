using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage")]
public class AzStorageContainer
{
    public AzStorageContainer(
        AzStorageContainerGenerateSas generateSas,
        AzStorageContainerImmutabilityPolicy immutabilityPolicy,
        AzStorageContainerLease lease,
        AzStorageContainerLegalHold legalHold,
        AzStorageContainerMetadata metadata,
        AzStorageContainerPolicy policy,
        ICommand internalCommand
    )
    {
        GenerateSasCommands = generateSas;
        ImmutabilityPolicy = immutabilityPolicy;
        Lease = lease;
        LegalHold = legalHold;
        Metadata = metadata;
        Policy = policy;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzStorageContainerGenerateSas GenerateSasCommands { get; }

    public AzStorageContainerImmutabilityPolicy ImmutabilityPolicy { get; }

    public AzStorageContainerLease Lease { get; }

    public AzStorageContainerLegalHold LegalHold { get; }

    public AzStorageContainerMetadata Metadata { get; }

    public AzStorageContainerPolicy Policy { get; }

    public async Task<CommandResult> Create(AzStorageContainerCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzStorageContainerDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Exists(AzStorageContainerExistsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GenerateSas(AzStorageContainerGenerateSasOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzStorageContainerListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Restore(AzStorageContainerRestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetPermission(AzStorageContainerSetPermissionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzStorageContainerShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ShowPermission(AzStorageContainerShowPermissionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}