using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzFunctionapp
{
    public AzFunctionapp(
        AzFunctionappApp app,
        AzFunctionappConfig config,
        AzFunctionappConnection connection,
        AzFunctionappCors cors,
        AzFunctionappCreate create,
        AzFunctionappDeployment deployment,
        AzFunctionappDevopsPipeline devopsPipeline,
        AzFunctionappFunction function,
        AzFunctionappHybridConnection hybridConnection,
        AzFunctionappIdentity identity,
        AzFunctionappKeys keys,
        AzFunctionappLog log,
        AzFunctionappPlan plan,
        AzFunctionappRestart restart,
        AzFunctionappShow show,
        AzFunctionappVnetIntegration vnetIntegration,
        ICommand internalCommand
    )
    {
        App = app;
        Config = config;
        Connection = connection;
        Cors = cors;
        CreateCommands = create;
        Deployment = deployment;
        DevopsPipeline = devopsPipeline;
        Function = function;
        HybridConnection = hybridConnection;
        Identity = identity;
        Keys = keys;
        Log = log;
        Plan = plan;
        RestartCommands = restart;
        ShowCommands = show;
        VnetIntegration = vnetIntegration;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzFunctionappApp App { get; }

    public AzFunctionappConfig Config { get; }

    public AzFunctionappConnection Connection { get; }

    public AzFunctionappCors Cors { get; }

    public AzFunctionappCreate CreateCommands { get; }

    public AzFunctionappDeployment Deployment { get; }

    public AzFunctionappDevopsPipeline DevopsPipeline { get; }

    public AzFunctionappFunction Function { get; }

    public AzFunctionappHybridConnection HybridConnection { get; }

    public AzFunctionappIdentity Identity { get; }

    public AzFunctionappKeys Keys { get; }

    public AzFunctionappLog Log { get; }

    public AzFunctionappPlan Plan { get; }

    public AzFunctionappRestart RestartCommands { get; }

    public AzFunctionappShow ShowCommands { get; }

    public AzFunctionappVnetIntegration VnetIntegration { get; }

    public async Task<CommandResult> Create(AzFunctionappCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzFunctionappDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappDeleteOptions(), token);
    }

    public async Task<CommandResult> Deploy(AzFunctionappDeployOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappDeployOptions(), token);
    }

    public async Task<CommandResult> List(AzFunctionappListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappListOptions(), token);
    }

    public async Task<CommandResult> ListConsumptionLocations(AzFunctionappListConsumptionLocationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappListConsumptionLocationsOptions(), token);
    }

    public async Task<CommandResult> ListRuntimes(AzFunctionappListRuntimesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappListRuntimesOptions(), token);
    }

    public async Task<CommandResult> Restart(AzFunctionappRestartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappRestartOptions(), token);
    }

    public async Task<CommandResult> Show(AzFunctionappShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappShowOptions(), token);
    }

    public async Task<CommandResult> Start(AzFunctionappStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappStartOptions(), token);
    }

    public async Task<CommandResult> Stop(AzFunctionappStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappStopOptions(), token);
    }

    public async Task<CommandResult> Update(AzFunctionappUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappUpdateOptions(), token);
    }
}