using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dla")]
public class AzDlaAccount
{
    public AzDlaAccount(
        AzDlaAccountBlobStorage blobStorage,
        AzDlaAccountComputePolicy computePolicy,
        AzDlaAccountDataLakeStore dataLakeStore,
        AzDlaAccountFirewall firewall,
        ICommand internalCommand
    )
    {
        BlobStorage = blobStorage;
        ComputePolicy = computePolicy;
        DataLakeStore = dataLakeStore;
        Firewall = firewall;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzDlaAccountBlobStorage BlobStorage { get; }

    public AzDlaAccountComputePolicy ComputePolicy { get; }

    public AzDlaAccountDataLakeStore DataLakeStore { get; }

    public AzDlaAccountFirewall Firewall { get; }

    public async Task<CommandResult> Create(AzDlaAccountCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDlaAccountDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDlaAccountDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzDlaAccountListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDlaAccountListOptions(), token);
    }

    public async Task<CommandResult> Show(AzDlaAccountShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDlaAccountShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzDlaAccountUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDlaAccountUpdateOptions(), token);
    }
}