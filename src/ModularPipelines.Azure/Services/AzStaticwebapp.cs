using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzStaticwebapp
{
    public AzStaticwebapp(
        AzStaticwebappAppsettings appsettings,
        AzStaticwebappBackends backends,
        AzStaticwebappDbconnection dbconnection,
        AzStaticwebappEnterpriseEdge enterpriseEdge,
        AzStaticwebappEnvironment environment,
        AzStaticwebappFunctions functions,
        AzStaticwebappHostname hostname,
        AzStaticwebappIdentity identity,
        AzStaticwebappSecrets secrets,
        AzStaticwebappUsers users,
        ICommand internalCommand
    )
    {
        Appsettings = appsettings;
        Backends = backends;
        Dbconnection = dbconnection;
        EnterpriseEdge = enterpriseEdge;
        Environment = environment;
        Functions = functions;
        Hostname = hostname;
        Identity = identity;
        Secrets = secrets;
        Users = users;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzStaticwebappAppsettings Appsettings { get; }

    public AzStaticwebappBackends Backends { get; }

    public AzStaticwebappDbconnection Dbconnection { get; }

    public AzStaticwebappEnterpriseEdge EnterpriseEdge { get; }

    public AzStaticwebappEnvironment Environment { get; }

    public AzStaticwebappFunctions Functions { get; }

    public AzStaticwebappHostname Hostname { get; }

    public AzStaticwebappIdentity Identity { get; }

    public AzStaticwebappSecrets Secrets { get; }

    public AzStaticwebappUsers Users { get; }

    public async Task<CommandResult> Create(AzStaticwebappCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzStaticwebappDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Disconnect(AzStaticwebappDisconnectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzStaticwebappListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStaticwebappListOptions(), token);
    }

    public async Task<CommandResult> Reconnect(AzStaticwebappReconnectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzStaticwebappShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzStaticwebappUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}