using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "log-analytics")]
public class AzMonitorLogAnalyticsWorkspace
{
    public AzMonitorLogAnalyticsWorkspace(
        AzMonitorLogAnalyticsWorkspaceDataExport dataExport,
        AzMonitorLogAnalyticsWorkspaceLinkedService linkedService,
        AzMonitorLogAnalyticsWorkspaceLinkedStorage linkedStorage,
        AzMonitorLogAnalyticsWorkspacePack pack,
        AzMonitorLogAnalyticsWorkspaceSavedSearch savedSearch,
        AzMonitorLogAnalyticsWorkspaceTable table,
        ICommand internalCommand
    )
    {
        DataExport = dataExport;
        LinkedService = linkedService;
        LinkedStorage = linkedStorage;
        Pack = pack;
        SavedSearch = savedSearch;
        Table = table;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzMonitorLogAnalyticsWorkspaceDataExport DataExport { get; }

    public AzMonitorLogAnalyticsWorkspaceLinkedService LinkedService { get; }

    public AzMonitorLogAnalyticsWorkspaceLinkedStorage LinkedStorage { get; }

    public AzMonitorLogAnalyticsWorkspacePack Pack { get; }

    public AzMonitorLogAnalyticsWorkspaceSavedSearch SavedSearch { get; }

    public AzMonitorLogAnalyticsWorkspaceTable Table { get; }

    public async Task<CommandResult> Create(AzMonitorLogAnalyticsWorkspaceCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzMonitorLogAnalyticsWorkspaceDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSchema(AzMonitorLogAnalyticsWorkspaceGetSchemaOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSharedKeys(AzMonitorLogAnalyticsWorkspaceGetSharedKeysOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzMonitorLogAnalyticsWorkspaceListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListDeletedWorkspaces(AzMonitorLogAnalyticsWorkspaceListDeletedWorkspacesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListManagementGroups(AzMonitorLogAnalyticsWorkspaceListManagementGroupsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListUsages(AzMonitorLogAnalyticsWorkspaceListUsagesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Recover(AzMonitorLogAnalyticsWorkspaceRecoverOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzMonitorLogAnalyticsWorkspaceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorLogAnalyticsWorkspaceShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzMonitorLogAnalyticsWorkspaceUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorLogAnalyticsWorkspaceUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzMonitorLogAnalyticsWorkspaceWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorLogAnalyticsWorkspaceWaitOptions(), token);
    }
}