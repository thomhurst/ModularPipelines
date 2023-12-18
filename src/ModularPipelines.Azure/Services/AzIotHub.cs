using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot")]
public class AzIotHub
{
    public AzIotHub(
        AzIotHubCertificate certificate,
        AzIotHubConfiguration configuration,
        AzIotHubConnectionString connectionString,
        AzIotHubConsumerGroup consumerGroup,
        AzIotHubDeviceIdentity deviceIdentity,
        AzIotHubDeviceTwin deviceTwin,
        AzIotHubDevicestream devicestream,
        AzIotHubDigitalTwin digitalTwin,
        AzIotHubDistributedTracing distributedTracing,
        AzIotHubIdentity identity,
        AzIotHubJob job,
        AzIotHubMessageEndpoint messageEndpoint,
        AzIotHubMessageEnrichment messageEnrichment,
        AzIotHubMessageRoute messageRoute,
        AzIotHubModuleIdentity moduleIdentity,
        AzIotHubModuleTwin moduleTwin,
        AzIotHubPolicy policy,
        AzIotHubRoute route,
        AzIotHubRoutingEndpoint routingEndpoint,
        AzIotHubState state,
        ICommand internalCommand
    )
    {
        Certificate = certificate;
        Configuration = configuration;
        ConnectionString = connectionString;
        ConsumerGroup = consumerGroup;
        DeviceIdentity = deviceIdentity;
        DeviceTwin = deviceTwin;
        Devicestream = devicestream;
        DigitalTwin = digitalTwin;
        DistributedTracing = distributedTracing;
        Identity = identity;
        Job = job;
        MessageEndpoint = messageEndpoint;
        MessageEnrichment = messageEnrichment;
        MessageRoute = messageRoute;
        ModuleIdentity = moduleIdentity;
        ModuleTwin = moduleTwin;
        Policy = policy;
        Route = route;
        RoutingEndpoint = routingEndpoint;
        State = state;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzIotHubCertificate Certificate { get; }

    public AzIotHubConfiguration Configuration { get; }

    public AzIotHubConnectionString ConnectionString { get; }

    public AzIotHubConsumerGroup ConsumerGroup { get; }

    public AzIotHubDeviceIdentity DeviceIdentity { get; }

    public AzIotHubDeviceTwin DeviceTwin { get; }

    public AzIotHubDevicestream Devicestream { get; }

    public AzIotHubDigitalTwin DigitalTwin { get; }

    public AzIotHubDistributedTracing DistributedTracing { get; }

    public AzIotHubIdentity Identity { get; }

    public AzIotHubJob Job { get; }

    public AzIotHubMessageEndpoint MessageEndpoint { get; }

    public AzIotHubMessageEnrichment MessageEnrichment { get; }

    public AzIotHubMessageRoute MessageRoute { get; }

    public AzIotHubModuleIdentity ModuleIdentity { get; }

    public AzIotHubModuleTwin ModuleTwin { get; }

    public AzIotHubPolicy Policy { get; }

    public AzIotHubRoute Route { get; }

    public AzIotHubRoutingEndpoint RoutingEndpoint { get; }

    public AzIotHubState State { get; }

    public async Task<CommandResult> Create(AzIotHubCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzIotHubDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GenerateSasToken(AzIotHubGenerateSasTokenOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> InvokeDeviceMethod(AzIotHubInvokeDeviceMethodOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> InvokeModuleMethod(AzIotHubInvokeModuleMethodOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzIotHubListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSkus(AzIotHubListSkusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ManualFailover(AzIotHubManualFailoverOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> MonitorEvents(AzIotHubMonitorEventsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> MonitorFeedback(AzIotHubMonitorFeedbackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Query(AzIotHubQueryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzIotHubShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzIotHubShowOptions(), token);
    }

    public async Task<CommandResult> ShowConnectionString(AzIotHubShowConnectionStringOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzIotHubShowConnectionStringOptions(), token);
    }

    public async Task<CommandResult> ShowQuotaMetrics(AzIotHubShowQuotaMetricsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzIotHubShowQuotaMetricsOptions(), token);
    }

    public async Task<CommandResult> ShowStats(AzIotHubShowStatsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzIotHubShowStatsOptions(), token);
    }

    public async Task<CommandResult> Update(AzIotHubUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzIotHubUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzIotHubWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzIotHubWaitOptions(), token);
    }
}

