using ModularPipelines.Docker.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Services;

public interface IDocker
{
    DockerNetwork DockerNetwork { get; }

    DockerCompose DockerCompose { get; }

    DockerVolume Volume { get; }

    DockerSwarm DockerSwarm { get; }

    DockerTrust DockerTrust { get; }

    DockerPlugin DockerPlugin { get; }

    DockerManifest DockerManifest { get; }

    DockerSecret DockerSecret { get; }

    DockerSystem DockerSystem { get; }

    DockerBuildx DockerBuildx { get; }

    DockerImage DockerImage { get; }

    DockerStack DockerStack { get; }

    DockerCheckpoint DockerCheckpoint { get; }

    DockerContainer DockerContainer { get; }

    DockerBuilder DockerBuilder { get; }

    DockerService DockerService { get; }

    DockerContext DockerContext { get; }

    DockerNode DockerNode { get; }

    DockerScout DockerScout { get; }

    DockerConfig DockerConfig { get; }

    Task<CommandResult> Builder(DockerBuilderOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Buildx(DockerBuildxOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Checkpoint(DockerCheckpointOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Compose(DockerComposeOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Config(DockerConfigOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Container(DockerContainerOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Context(DockerContextOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Image(DockerImageOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Init(DockerInitOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Inspect(DockerInspectOptions options, CancellationToken token = default);

    Task<CommandResult> Login(DockerLoginOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Logout(DockerLogoutOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Manifest(DockerManifestOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Network(DockerNetworkOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Node(DockerNodeOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Plugin(DockerPluginOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Scout(DockerScoutOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Search(DockerSearchOptions options, CancellationToken token = default);

    Task<CommandResult> Secret(DockerSecretOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Service(DockerServiceOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Stack(DockerStackOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Swarm(DockerSwarmOptions? options = default, CancellationToken token = default);

    Task<CommandResult> System(DockerSystemOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Trust(DockerTrustOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Version(DockerVersionOptions? options = default, CancellationToken token = default);
}