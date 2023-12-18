using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzAks
{
    public AzAks(
        AzAksAddon addon,
        AzAksApp app,
        AzAksApprouting approuting,
        AzAksBrowse browse,
        AzAksCommand command,
        AzAksCreate create,
        AzAksDelete delete,
        AzAksDisableAddons disableAddons,
        AzAksDraft draft,
        AzAksEgressEndpoints egressEndpoints,
        AzAksEnableAddons enableAddons,
        AzAksGetCredentials getCredentials,
        AzAksGetUpgrades getUpgrades,
        AzAksGetVersions getVersions,
        AzAksList list,
        AzAksMachine machine,
        AzAksMaintenanceconfiguration maintenanceconfiguration,
        AzAksMesh mesh,
        AzAksNodepool nodepool,
        AzAksOidcIssuer oidcIssuer,
        AzAksOperationAbort operationAbort,
        AzAksPodIdentity podIdentity,
        AzAksRotateCerts rotateCerts,
        AzAksScale scale,
        AzAksShow show,
        AzAksSnapshot snapshot,
        AzAksStart start,
        AzAksStop stop,
        AzAksTrustedaccess trustedaccess,
        AzAksUpdate update,
        AzAksUpgrade upgrade,
        AzAksUseDevSpaces useDevSpaces,
        AzAksWait wait,
        ICommand internalCommand
    )
    {
        Addon = addon;
        App = app;
        Approuting = approuting;
        BrowseCommands = browse;
        Command = command;
        CreateCommands = create;
        DeleteCommands = delete;
        DisableAddonsCommands = disableAddons;
        Draft = draft;
        EgressEndpoints = egressEndpoints;
        EnableAddonsCommands = enableAddons;
        GetCredentialsCommands = getCredentials;
        GetUpgradesCommands = getUpgrades;
        GetVersionsCommands = getVersions;
        ListCommands = list;
        Machine = machine;
        Maintenanceconfiguration = maintenanceconfiguration;
        Mesh = mesh;
        Nodepool = nodepool;
        OidcIssuer = oidcIssuer;
        OperationAbortCommands = operationAbort;
        PodIdentity = podIdentity;
        RotateCertsCommands = rotateCerts;
        ScaleCommands = scale;
        ShowCommands = show;
        Snapshot = snapshot;
        StartCommands = start;
        StopCommands = stop;
        Trustedaccess = trustedaccess;
        UpdateCommands = update;
        UpgradeCommands = upgrade;
        UseDevSpaces = useDevSpaces;
        WaitCommands = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAksAddon Addon { get; }

    public AzAksApp App { get; }

    public AzAksApprouting Approuting { get; }

    public AzAksBrowse BrowseCommands { get; }

    public AzAksCommand Command { get; }

    public AzAksCreate CreateCommands { get; }

    public AzAksDelete DeleteCommands { get; }

    public AzAksDisableAddons DisableAddonsCommands { get; }

    public AzAksDraft Draft { get; }

    public AzAksEgressEndpoints EgressEndpoints { get; }

    public AzAksEnableAddons EnableAddonsCommands { get; }

    public AzAksGetCredentials GetCredentialsCommands { get; }

    public AzAksGetUpgrades GetUpgradesCommands { get; }

    public AzAksGetVersions GetVersionsCommands { get; }

    public AzAksList ListCommands { get; }

    public AzAksMachine Machine { get; }

    public AzAksMaintenanceconfiguration Maintenanceconfiguration { get; }

    public AzAksMesh Mesh { get; }

    public AzAksNodepool Nodepool { get; }

    public AzAksOidcIssuer OidcIssuer { get; }

    public AzAksOperationAbort OperationAbortCommands { get; }

    public AzAksPodIdentity PodIdentity { get; }

    public AzAksRotateCerts RotateCertsCommands { get; }

    public AzAksScale ScaleCommands { get; }

    public AzAksShow ShowCommands { get; }

    public AzAksSnapshot Snapshot { get; }

    public AzAksStart StartCommands { get; }

    public AzAksStop StopCommands { get; }

    public AzAksTrustedaccess Trustedaccess { get; }

    public AzAksUpdate UpdateCommands { get; }

    public AzAksUpgrade UpgradeCommands { get; }

    public AzAksUseDevSpaces UseDevSpaces { get; }

    public AzAksWait WaitCommands { get; }

    public async Task<CommandResult> Browse(AzAksBrowseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CheckAcr(AzAksCheckAcrOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzAksCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAksDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisableAddons(AzAksDisableAddonsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableAddons(AzAksEnableAddonsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCredentials(AzAksGetCredentialsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetOsOptions(AzAksGetOsOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetUpgrades(AzAksGetUpgradesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetVersions(AzAksGetVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> InstallCli(AzAksInstallCliOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAksInstallCliOptions(), token);
    }

    public async Task<CommandResult> Kanalyze(AzAksKanalyzeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Kollect(AzAksKollectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzAksListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAksListOptions(), token);
    }

    public async Task<CommandResult> OperationAbort(AzAksOperationAbortOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RotateCerts(AzAksRotateCertsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Scale(AzAksScaleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzAksShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Start(AzAksStartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Stop(AzAksStopOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzAksUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateCredentials(AzAksUpdateCredentialsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Upgrade(AzAksUpgradeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzAksWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}