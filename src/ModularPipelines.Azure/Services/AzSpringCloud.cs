using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzSpringCloud
{
    public AzSpringCloud(
        AzSpringCloudApiPortal apiPortal,
        AzSpringCloudApp app,
        AzSpringCloudAppInsights appInsights,
        AzSpringCloudApplicationConfigurationService applicationConfigurationService,
        AzSpringCloudBuildService buildService,
        AzSpringCloudCertificate certificate,
        AzSpringCloudConfigServer configServer,
        AzSpringCloudConnection connection,
        AzSpringCloudGateway gateway,
        AzSpringCloudServiceRegistry serviceRegistry,
        AzSpringCloudStorage storage,
        AzSpringCloudTestEndpoint testEndpoint,
        ICommand internalCommand
    )
    {
        ApiPortal = apiPortal;
        App = app;
        AppInsights = appInsights;
        ApplicationConfigurationService = applicationConfigurationService;
        BuildService = buildService;
        Certificate = certificate;
        ConfigServer = configServer;
        Connection = connection;
        Gateway = gateway;
        ServiceRegistry = serviceRegistry;
        Storage = storage;
        TestEndpoint = testEndpoint;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSpringCloudApiPortal ApiPortal { get; }

    public AzSpringCloudApp App { get; }

    public AzSpringCloudAppInsights AppInsights { get; }

    public AzSpringCloudApplicationConfigurationService ApplicationConfigurationService { get; }

    public AzSpringCloudBuildService BuildService { get; }

    public AzSpringCloudCertificate Certificate { get; }

    public AzSpringCloudConfigServer ConfigServer { get; }

    public AzSpringCloudConnection Connection { get; }

    public AzSpringCloudGateway Gateway { get; }

    public AzSpringCloudServiceRegistry ServiceRegistry { get; }

    public AzSpringCloudStorage Storage { get; }

    public AzSpringCloudTestEndpoint TestEndpoint { get; }

    public async Task<CommandResult> Create(AzSpringCloudCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSpringCloudDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzSpringCloudListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSpringCloudShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Start(AzSpringCloudStartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Stop(AzSpringCloudStopOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzSpringCloudUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}

