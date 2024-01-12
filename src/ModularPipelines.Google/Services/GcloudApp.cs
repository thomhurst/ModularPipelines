using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudApp
{
    public GcloudApp(
        GcloudAppDomainMappings domainMappings,
        GcloudAppFirewallRules firewallRules,
        GcloudAppInstances instances,
        GcloudAppLogs logs,
        GcloudAppOperations operations,
        GcloudAppRegions regions,
        GcloudAppRuntimes runtimes,
        GcloudAppServices services,
        GcloudAppSslCertificates sslCertificates,
        GcloudAppVersions versions,
        ICommand internalCommand
    )
    {
        DomainMappings = domainMappings;
        FirewallRules = firewallRules;
        Instances = instances;
        Logs = logs;
        Operations = operations;
        Regions = regions;
        Runtimes = runtimes;
        Services = services;
        SslCertificates = sslCertificates;
        Versions = versions;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudAppDomainMappings DomainMappings { get; }

    public GcloudAppFirewallRules FirewallRules { get; }

    public GcloudAppInstances Instances { get; }

    public GcloudAppLogs Logs { get; }

    public GcloudAppOperations Operations { get; }

    public GcloudAppRegions Regions { get; }

    public GcloudAppRuntimes Runtimes { get; }

    public GcloudAppServices Services { get; }

    public GcloudAppSslCertificates SslCertificates { get; }

    public GcloudAppVersions Versions { get; }

    public async Task<CommandResult> Browse(GcloudAppBrowseOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudAppBrowseOptions(), token);
    }

    public async Task<CommandResult> Create(GcloudAppCreateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudAppCreateOptions(), token);
    }

    public async Task<CommandResult> Deploy(GcloudAppDeployOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudAppDescribeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudAppDescribeOptions(), token);
    }

    public async Task<CommandResult> OpenConsole(GcloudAppOpenConsoleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudAppOpenConsoleOptions(), token);
    }

    public async Task<CommandResult> Update(GcloudAppUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudAppUpdateOptions(), token);
    }
}