using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("devcenter", "admin")]
public class AzDevcenterAdminCatalog
{
    public AzDevcenterAdminCatalog(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzDevcenterAdminCatalogCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzDevcenterAdminCatalogDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevcenterAdminCatalogDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> GetSyncErrorDetail(AzDevcenterAdminCatalogGetSyncErrorDetailOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevcenterAdminCatalogGetSyncErrorDetailOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzDevcenterAdminCatalogListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzDevcenterAdminCatalogShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevcenterAdminCatalogShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Sync(AzDevcenterAdminCatalogSyncOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevcenterAdminCatalogSyncOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzDevcenterAdminCatalogUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevcenterAdminCatalogUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzDevcenterAdminCatalogWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevcenterAdminCatalogWaitOptions(), cancellationToken: token);
    }
}