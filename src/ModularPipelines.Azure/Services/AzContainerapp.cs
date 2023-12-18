using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzContainerapp
{
    public AzContainerapp(
        AzContainerappAddOn addOn,
        AzContainerappAuth auth,
        AzContainerappCompose compose,
        AzContainerappConnectedEnv connectedEnv,
        AzContainerappConnection connection,
        AzContainerappDapr dapr,
        AzContainerappEnv env,
        AzContainerappGithubAction githubAction,
        AzContainerappHostname hostname,
        AzContainerappIdentity identity,
        AzContainerappIngress ingress,
        AzContainerappJob job,
        AzContainerappLogs logs,
        AzContainerappPatch patch,
        AzContainerappRegistry registry,
        AzContainerappReplica replica,
        AzContainerappResiliency resiliency,
        AzContainerappRevision revision,
        AzContainerappSecret secret,
        AzContainerappSsl ssl,
        ICommand internalCommand
    )
    {
        AddOn = addOn;
        Auth = auth;
        Compose = compose;
        ConnectedEnv = connectedEnv;
        Connection = connection;
        Dapr = dapr;
        Env = env;
        GithubAction = githubAction;
        Hostname = hostname;
        Identity = identity;
        Ingress = ingress;
        Job = job;
        Logs = logs;
        Patch = patch;
        Registry = registry;
        Replica = replica;
        Resiliency = resiliency;
        Revision = revision;
        Secret = secret;
        Ssl = ssl;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzContainerappAddOn AddOn { get; }

    public AzContainerappAuth Auth { get; }

    public AzContainerappCompose Compose { get; }

    public AzContainerappConnectedEnv ConnectedEnv { get; }

    public AzContainerappConnection Connection { get; }

    public AzContainerappDapr Dapr { get; }

    public AzContainerappEnv Env { get; }

    public AzContainerappGithubAction GithubAction { get; }

    public AzContainerappHostname Hostname { get; }

    public AzContainerappIdentity Identity { get; }

    public AzContainerappIngress Ingress { get; }

    public AzContainerappJob Job { get; }

    public AzContainerappLogs Logs { get; }

    public AzContainerappPatch Patch { get; }

    public AzContainerappRegistry Registry { get; }

    public AzContainerappReplica Replica { get; }

    public AzContainerappResiliency Resiliency { get; }

    public AzContainerappRevision Revision { get; }

    public AzContainerappSecret Secret { get; }

    public AzContainerappSsl Ssl { get; }

    public async Task<CommandResult> Browse(AzContainerappBrowseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzContainerappCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzContainerappDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Exec(AzContainerappExecOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzContainerappListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListUsages(AzContainerappListUsagesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzContainerappShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ShowCustomDomainVerificationId(AzContainerappShowCustomDomainVerificationIdOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Up(AzContainerappUpOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzContainerappUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappUpdateOptions(), token);
    }
}