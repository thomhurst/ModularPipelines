using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzSpring
{
    public AzSpring(
        AzSpringApiPortal apiPortal,
        AzSpringApm apm,
        AzSpringApp app,
        AzSpringAppInsights appInsights,
        AzSpringApplicationAccelerator applicationAccelerator,
        AzSpringApplicationConfigurationService applicationConfigurationService,
        AzSpringApplicationLiveView applicationLiveView,
        AzSpringBuildService buildService,
        AzSpringCertificate certificate,
        AzSpringConfigServer configServer,
        AzSpringConnection connection,
        AzSpringContainerRegistry containerRegistry,
        AzSpringDevTool devTool,
        AzSpringEurekaServer eurekaServer,
        AzSpringGateway gateway,
        AzSpringServiceRegistry serviceRegistry,
        AzSpringStorage storage,
        AzSpringTestEndpoint testEndpoint,
        ICommand internalCommand
    )
    {
        ApiPortal = apiPortal;
        Apm = apm;
        App = app;
        AppInsights = appInsights;
        ApplicationAccelerator = applicationAccelerator;
        ApplicationConfigurationService = applicationConfigurationService;
        ApplicationLiveView = applicationLiveView;
        BuildService = buildService;
        Certificate = certificate;
        ConfigServer = configServer;
        Connection = connection;
        ContainerRegistry = containerRegistry;
        DevTool = devTool;
        EurekaServer = eurekaServer;
        Gateway = gateway;
        ServiceRegistry = serviceRegistry;
        Storage = storage;
        TestEndpoint = testEndpoint;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSpringApiPortal ApiPortal { get; }

    public AzSpringApm Apm { get; }

    public AzSpringApp App { get; }

    public AzSpringAppInsights AppInsights { get; }

    public AzSpringApplicationAccelerator ApplicationAccelerator { get; }

    public AzSpringApplicationConfigurationService ApplicationConfigurationService { get; }

    public AzSpringApplicationLiveView ApplicationLiveView { get; }

    public AzSpringBuildService BuildService { get; }

    public AzSpringCertificate Certificate { get; }

    public AzSpringConfigServer ConfigServer { get; }

    public AzSpringConnection Connection { get; }

    public AzSpringContainerRegistry ContainerRegistry { get; }

    public AzSpringDevTool DevTool { get; }

    public AzSpringEurekaServer EurekaServer { get; }

    public AzSpringGateway Gateway { get; }

    public AzSpringServiceRegistry ServiceRegistry { get; }

    public AzSpringStorage Storage { get; }

    public AzSpringTestEndpoint TestEndpoint { get; }

    public async Task<CommandResult> Create(AzSpringCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSpringDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> FlushVirtualnetworkDnsSettings(AzSpringFlushVirtualnetworkDnsSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzSpringListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListMarketplacePlan(AzSpringListMarketplacePlanOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSupportServerVersions(AzSpringListSupportServerVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSpringShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Start(AzSpringStartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Stop(AzSpringStopOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzSpringUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}

