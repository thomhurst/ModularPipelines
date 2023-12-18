using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzApim
{
    public AzApim(
        AzApimApi api,
        AzApimDeletedservice deletedservice,
        AzApimGraphql graphql,
        AzApimNv nv,
        AzApimProduct product,
        ICommand internalCommand
    )
    {
        Api = api;
        Deletedservice = deletedservice;
        Graphql = graphql;
        Nv = nv;
        Product = product;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzApimApi Api { get; }

    public AzApimDeletedservice Deletedservice { get; }

    public AzApimGraphql Graphql { get; }

    public AzApimNv Nv { get; }

    public AzApimProduct Product { get; }

    public async Task<CommandResult> ApplyNetworkUpdates(AzApimApplyNetworkUpdatesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Backup(AzApimBackupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CheckName(AzApimCheckNameOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzApimCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzApimDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzApimListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Restore(AzApimRestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzApimShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzApimUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzApimWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}