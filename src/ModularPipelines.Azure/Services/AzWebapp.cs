using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzWebapp
{
    public AzWebapp(
        AzWebappAuth auth,
        AzWebappAuthClassic authClassic,
        AzWebappConfig config,
        AzWebappConnection connection,
        AzWebappCors cors,
        AzWebappCreate create,
        AzWebappDeleted deleted,
        AzWebappDeployment deployment,
        AzWebappHybridConnection hybridConnection,
        AzWebappIdentity identity,
        AzWebappLog log,
        AzWebappRestart restart,
        AzWebappScan scan,
        AzWebappShow show,
        AzWebappTrafficRouting trafficRouting,
        AzWebappUpdate update,
        AzWebappVnetIntegration vnetIntegration,
        AzWebappWebjob webjob,
        ICommand internalCommand
    )
    {
        Auth = auth;
        AuthClassic = authClassic;
        Config = config;
        Connection = connection;
        Cors = cors;
        CreateCommands = create;
        Deleted = deleted;
        Deployment = deployment;
        HybridConnection = hybridConnection;
        Identity = identity;
        Log = log;
        RestartCommands = restart;
        Scan = scan;
        ShowCommands = show;
        TrafficRouting = trafficRouting;
        UpdateCommands = update;
        VnetIntegration = vnetIntegration;
        Webjob = webjob;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzWebappAuth Auth { get; }

    public AzWebappAuthClassic AuthClassic { get; }

    public AzWebappConfig Config { get; }

    public AzWebappConnection Connection { get; }

    public AzWebappCors Cors { get; }

    public AzWebappCreate CreateCommands { get; }

    public AzWebappDeleted Deleted { get; }

    public AzWebappDeployment Deployment { get; }

    public AzWebappHybridConnection HybridConnection { get; }

    public AzWebappIdentity Identity { get; }

    public AzWebappLog Log { get; }

    public AzWebappRestart RestartCommands { get; }

    public AzWebappScan Scan { get; }

    public AzWebappShow ShowCommands { get; }

    public AzWebappTrafficRouting TrafficRouting { get; }

    public AzWebappUpdate UpdateCommands { get; }

    public AzWebappVnetIntegration VnetIntegration { get; }

    public AzWebappWebjob Webjob { get; }

    public async Task<CommandResult> Browse(AzWebappBrowseOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappBrowseOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Create(AzWebappCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> CreateRemoteConnection(AzWebappCreateRemoteConnectionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappCreateRemoteConnectionOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzWebappDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Deploy(AzWebappDeployOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappDeployOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzWebappListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListInstances(AzWebappListInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> ListRuntimes(AzWebappListRuntimesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappListRuntimesOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Restart(AzWebappRestartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappRestartOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Scale(AzWebappScaleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzWebappShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Ssh(AzWebappSshOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappSshOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Start(AzWebappStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappStartOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Stop(AzWebappStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappStopOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Up(AzWebappUpOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappUpOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzWebappUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappUpdateOptions(), cancellationToken: token);
    }
}