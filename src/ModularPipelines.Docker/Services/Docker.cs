using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Docker.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Services;

[ExcludeFromCodeCoverage]
public class Docker : IDocker
{
    public Docker(
        DockerNetwork dockerNetwork,
        DockerCompose dockerCompose,
        DockerVolume volume,
        DockerSwarm dockerSwarm,
        DockerTrust dockerTrust,
        DockerPlugin dockerPlugin,
        DockerManifest dockerManifest,
        DockerSecret dockerSecret,
        DockerSystem dockerSystem,
        DockerBuildx dockerBuildx,
        DockerImage dockerImage,
        DockerStack dockerStack,
        DockerCheckpoint dockerCheckpoint,
        DockerContainer dockerContainer,
        DockerBuilder dockerBuilder,
        DockerService dockerService,
        DockerContext dockerContext,
        DockerNode dockerNode,
        DockerScout dockerScout,
        DockerConfig dockerConfig,
        ICommand internalCommand
    )
    {
        DockerNetwork = dockerNetwork;
        DockerCompose = dockerCompose;
        Volume = volume;
        DockerSwarm = dockerSwarm;
        DockerTrust = dockerTrust;
        DockerPlugin = dockerPlugin;
        DockerManifest = dockerManifest;
        DockerSecret = dockerSecret;
        DockerSystem = dockerSystem;
        DockerBuildx = dockerBuildx;
        DockerImage = dockerImage;
        DockerStack = dockerStack;
        DockerCheckpoint = dockerCheckpoint;
        DockerContainer = dockerContainer;
        DockerBuilder = dockerBuilder;
        DockerService = dockerService;
        DockerContext = dockerContext;
        DockerNode = dockerNode;
        DockerScout = dockerScout;
        DockerConfig = dockerConfig;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public DockerNetwork DockerNetwork { get; }

    public DockerCompose DockerCompose { get; }

    public DockerVolume Volume { get; }

    public DockerSwarm DockerSwarm { get; }

    public DockerTrust DockerTrust { get; }

    public DockerPlugin DockerPlugin { get; }

    public DockerManifest DockerManifest { get; }

    public DockerSecret DockerSecret { get; }

    public DockerSystem DockerSystem { get; }

    public DockerBuildx DockerBuildx { get; }

    public DockerImage DockerImage { get; }

    public DockerStack DockerStack { get; }

    public DockerCheckpoint DockerCheckpoint { get; }

    public DockerContainer DockerContainer { get; }

    public DockerBuilder DockerBuilder { get; }

    public DockerService DockerService { get; }

    public DockerContext DockerContext { get; }

    public DockerNode DockerNode { get; }

    public DockerScout DockerScout { get; }

    public DockerConfig DockerConfig { get; }

    public async Task<CommandResult> Builder(DockerBuilderOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerBuilderOptions(), token);
    }

    public async Task<CommandResult> Buildx(DockerBuildxOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerBuildxOptions(), token);
    }

    public async Task<CommandResult> Checkpoint(DockerCheckpointOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerCheckpointOptions(), token);
    }

    public async Task<CommandResult> Compose(DockerComposeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeOptions(), token);
    }

    public async Task<CommandResult> Config(DockerConfigOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerConfigOptions(), token);
    }

    public async Task<CommandResult> Container(DockerContainerOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerContainerOptions(), token);
    }

    public async Task<CommandResult> Context(DockerContextOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerContextOptions(), token);
    }

    public async Task<CommandResult> Image(DockerImageOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerImageOptions(), token);
    }

    public async Task<CommandResult> Init(DockerInitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerInitOptions(), token);
    }

    public async Task<CommandResult> Inspect(DockerInspectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Login(DockerLoginOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerLoginOptions(), token);
    }

    public async Task<CommandResult> Logout(DockerLogoutOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerLogoutOptions(), token);
    }

    public async Task<CommandResult> Manifest(DockerManifestOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerManifestOptions(), token);
    }

    public async Task<CommandResult> Network(DockerNetworkOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerNetworkOptions(), token);
    }

    public async Task<CommandResult> Node(DockerNodeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerNodeOptions(), token);
    }

    public async Task<CommandResult> Plugin(DockerPluginOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerPluginOptions(), token);
    }

    public async Task<CommandResult> Scout(DockerScoutOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerScoutOptions(), token);
    }

    public async Task<CommandResult> Search(DockerSearchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Secret(DockerSecretOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerSecretOptions(), token);
    }

    public async Task<CommandResult> Service(DockerServiceOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerServiceOptions(), token);
    }

    public async Task<CommandResult> Stack(DockerStackOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerStackOptions(), token);
    }

    public async Task<CommandResult> Swarm(DockerSwarmOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerSwarmOptions(), token);
    }

    public async Task<CommandResult> System(DockerSystemOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerSystemOptions(), token);
    }

    public async Task<CommandResult> Trust(DockerTrustOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerTrustOptions(), token);
    }

    public async Task<CommandResult> Version(DockerVersionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerVersionOptions(), token);
    }
}
