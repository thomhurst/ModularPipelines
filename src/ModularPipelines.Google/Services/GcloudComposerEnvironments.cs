using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("composer")]
public class GcloudComposerEnvironments
{
    public GcloudComposerEnvironments(
        GcloudComposerEnvironmentsSnapshots snapshots,
        GcloudComposerEnvironmentsStorage storage,
        ICommand internalCommand
    )
    {
        Snapshots = snapshots;
        Storage = storage;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudComposerEnvironmentsSnapshots Snapshots { get; }

    public GcloudComposerEnvironmentsStorage Storage { get; }

    public async Task<CommandResult> Create(GcloudComposerEnvironmentsCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DatabaseFailover(GcloudComposerEnvironmentsDatabaseFailoverOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudComposerEnvironmentsDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudComposerEnvironmentsDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> FetchDatabaseProperties(GcloudComposerEnvironmentsFetchDatabasePropertiesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudComposerEnvironmentsListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudComposerEnvironmentsListOptions(), token);
    }

    public async Task<CommandResult> ListPackages(GcloudComposerEnvironmentsListPackagesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Run(GcloudComposerEnvironmentsRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudComposerEnvironmentsUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}