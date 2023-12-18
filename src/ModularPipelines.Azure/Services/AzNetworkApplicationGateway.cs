using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network")]
public class AzNetworkApplicationGateway
{
    public AzNetworkApplicationGateway(
        AzNetworkApplicationGatewayAddressPool addressPool,
        AzNetworkApplicationGatewayAuthCert authCert,
        AzNetworkApplicationGatewayClientCert clientCert,
        AzNetworkApplicationGatewayFrontendIp frontendIp,
        AzNetworkApplicationGatewayFrontendPort frontendPort,
        AzNetworkApplicationGatewayHttpListener httpListener,
        AzNetworkApplicationGatewayHttpSettings httpSettings,
        AzNetworkApplicationGatewayIdentity identity,
        AzNetworkApplicationGatewayListener listener,
        AzNetworkApplicationGatewayPrivateLink privateLink,
        AzNetworkApplicationGatewayProbe probe,
        AzNetworkApplicationGatewayRedirectConfig redirectConfig,
        AzNetworkApplicationGatewayRewriteRule rewriteRule,
        AzNetworkApplicationGatewayRootCert rootCert,
        AzNetworkApplicationGatewayRoutingRule routingRule,
        AzNetworkApplicationGatewayRule rule,
        AzNetworkApplicationGatewaySettings settings,
        AzNetworkApplicationGatewaySslCert sslCert,
        AzNetworkApplicationGatewaySslPolicy sslPolicy,
        AzNetworkApplicationGatewaySslProfile sslProfile,
        AzNetworkApplicationGatewayUrlPathMap urlPathMap,
        AzNetworkApplicationGatewayWafConfig wafConfig,
        AzNetworkApplicationGatewayWafPolicy wafPolicy,
        ICommand internalCommand
    )
    {
        AddressPool = addressPool;
        AuthCert = authCert;
        ClientCert = clientCert;
        FrontendIp = frontendIp;
        FrontendPort = frontendPort;
        HttpListener = httpListener;
        HttpSettings = httpSettings;
        Identity = identity;
        Listener = listener;
        PrivateLink = privateLink;
        Probe = probe;
        RedirectConfig = redirectConfig;
        RewriteRule = rewriteRule;
        RootCert = rootCert;
        RoutingRule = routingRule;
        Rule = rule;
        Settings = settings;
        SslCert = sslCert;
        SslPolicy = sslPolicy;
        SslProfile = sslProfile;
        UrlPathMap = urlPathMap;
        WafConfig = wafConfig;
        WafPolicy = wafPolicy;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkApplicationGatewayAddressPool AddressPool { get; }

    public AzNetworkApplicationGatewayAuthCert AuthCert { get; }

    public AzNetworkApplicationGatewayClientCert ClientCert { get; }

    public AzNetworkApplicationGatewayFrontendIp FrontendIp { get; }

    public AzNetworkApplicationGatewayFrontendPort FrontendPort { get; }

    public AzNetworkApplicationGatewayHttpListener HttpListener { get; }

    public AzNetworkApplicationGatewayHttpSettings HttpSettings { get; }

    public AzNetworkApplicationGatewayIdentity Identity { get; }

    public AzNetworkApplicationGatewayListener Listener { get; }

    public AzNetworkApplicationGatewayPrivateLink PrivateLink { get; }

    public AzNetworkApplicationGatewayProbe Probe { get; }

    public AzNetworkApplicationGatewayRedirectConfig RedirectConfig { get; }

    public AzNetworkApplicationGatewayRewriteRule RewriteRule { get; }

    public AzNetworkApplicationGatewayRootCert RootCert { get; }

    public AzNetworkApplicationGatewayRoutingRule RoutingRule { get; }

    public AzNetworkApplicationGatewayRule Rule { get; }

    public AzNetworkApplicationGatewaySettings Settings { get; }

    public AzNetworkApplicationGatewaySslCert SslCert { get; }

    public AzNetworkApplicationGatewaySslPolicy SslPolicy { get; }

    public AzNetworkApplicationGatewaySslProfile SslProfile { get; }

    public AzNetworkApplicationGatewayUrlPathMap UrlPathMap { get; }

    public AzNetworkApplicationGatewayWafConfig WafConfig { get; }

    public AzNetworkApplicationGatewayWafPolicy WafPolicy { get; }

    public async Task<CommandResult> Create(AzNetworkApplicationGatewayCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkApplicationGatewayDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkApplicationGatewayDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkApplicationGatewayListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkApplicationGatewayListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkApplicationGatewayShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkApplicationGatewayShowOptions(), token);
    }

    public async Task<CommandResult> ShowBackendHealth(AzNetworkApplicationGatewayShowBackendHealthOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkApplicationGatewayShowBackendHealthOptions(), token);
    }

    public async Task<CommandResult> Start(AzNetworkApplicationGatewayStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkApplicationGatewayStartOptions(), token);
    }

    public async Task<CommandResult> Stop(AzNetworkApplicationGatewayStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkApplicationGatewayStopOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkApplicationGatewayUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkApplicationGatewayUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkApplicationGatewayWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkApplicationGatewayWaitOptions(), token);
    }
}