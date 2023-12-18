using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzAcr
{
    public AzAcr(
        AzAcrAgentpool agentpool,
        AzAcrArtifactStreaming artifactStreaming,
        AzAcrCache cache,
        AzAcrConfig config,
        AzAcrConnectedRegistry connectedRegistry,
        AzAcrCredential credential,
        AzAcrCredentialSet credentialSet,
        AzAcrEncryption encryption,
        AzAcrExportPipeline exportPipeline,
        AzAcrHelm helm,
        AzAcrIdentity identity,
        AzAcrImportPipeline importPipeline,
        AzAcrManifest manifest,
        AzAcrNetworkRule networkRule,
        AzAcrPack pack,
        AzAcrPipelineRun pipelineRun,
        AzAcrPrivateEndpointConnection privateEndpointConnection,
        AzAcrPrivateLinkResource privateLinkResource,
        AzAcrReplication replication,
        AzAcrRepository repository,
        AzAcrScopeMap scopeMap,
        AzAcrTask task,
        AzAcrTaskrun taskrun,
        AzAcrToken token,
        AzAcrWebhook webhook,
        ICommand internalCommand
    )
    {
        Agentpool = agentpool;
        ArtifactStreaming = artifactStreaming;
        Cache = cache;
        Config = config;
        ConnectedRegistry = connectedRegistry;
        Credential = credential;
        CredentialSet = credentialSet;
        Encryption = encryption;
        ExportPipeline = exportPipeline;
        Helm = helm;
        Identity = identity;
        ImportPipeline = importPipeline;
        Manifest = manifest;
        NetworkRule = networkRule;
        Pack = pack;
        PipelineRun = pipelineRun;
        PrivateEndpointConnection = privateEndpointConnection;
        PrivateLinkResource = privateLinkResource;
        Replication = replication;
        Repository = repository;
        ScopeMap = scopeMap;
        Task = task;
        Taskrun = taskrun;
        Token = token;
        Webhook = webhook;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAcrAgentpool Agentpool { get; }

    public AzAcrArtifactStreaming ArtifactStreaming { get; }

    public AzAcrCache Cache { get; }

    public AzAcrConfig Config { get; }

    public AzAcrConnectedRegistry ConnectedRegistry { get; }

    public AzAcrCredential Credential { get; }

    public AzAcrCredentialSet CredentialSet { get; }

    public AzAcrEncryption Encryption { get; }

    public AzAcrExportPipeline ExportPipeline { get; }

    public AzAcrHelm Helm { get; }

    public AzAcrIdentity Identity { get; }

    public AzAcrImportPipeline ImportPipeline { get; }

    public AzAcrManifest Manifest { get; }

    public AzAcrNetworkRule NetworkRule { get; }

    public AzAcrPack Pack { get; }

    public AzAcrPipelineRun PipelineRun { get; }

    public AzAcrPrivateEndpointConnection PrivateEndpointConnection { get; }

    public AzAcrPrivateLinkResource PrivateLinkResource { get; }

    public AzAcrReplication Replication { get; }

    public AzAcrRepository Repository { get; }

    public AzAcrScopeMap ScopeMap { get; }

    public AzAcrTask Task { get; }

    public AzAcrTaskrun Taskrun { get; }

    public AzAcrToken Token { get; }

    public AzAcrWebhook Webhook { get; }

    public async Task<CommandResult> Build(AzAcrBuildOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CheckHealth(AzAcrCheckHealthOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CheckName(AzAcrCheckNameOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzAcrCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAcrDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Import(AzAcrImportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzAcrListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Login(AzAcrLoginOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Query(AzAcrQueryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Run(AzAcrRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzAcrShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ShowEndpoints(AzAcrShowEndpointsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ShowUsage(AzAcrShowUsageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzAcrUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}