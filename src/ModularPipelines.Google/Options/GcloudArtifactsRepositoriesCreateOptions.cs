using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "repositories", "create")]
public record GcloudArtifactsRepositoriesCreateOptions(
[property: PositionalArgument] string Repository,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--repository-format")] string RepositoryFormat
) : GcloudOptions
{
    [BooleanCommandSwitch("--allow-snapshot-overwrites")]
    public bool? AllowSnapshotOverwrites { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--immutable-tags")]
    public bool? ImmutableTags { get; set; }

    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--mode")]
    public string? Mode { get; set; }

    [CommandSwitch("--remote-apt-repo")]
    public string? RemoteAptRepo { get; set; }

    [CommandSwitch("--remote-apt-repo-path")]
    public string? RemoteAptRepoPath { get; set; }

    [CommandSwitch("--remote-docker-repo")]
    public string? RemoteDockerRepo { get; set; }

    [CommandSwitch("--remote-mvn-repo")]
    public string? RemoteMvnRepo { get; set; }

    [CommandSwitch("--remote-npm-repo")]
    public string? RemoteNpmRepo { get; set; }

    [CommandSwitch("--remote-password-secret-version")]
    public string? RemotePasswordSecretVersion { get; set; }

    [CommandSwitch("--remote-python-repo")]
    public string? RemotePythonRepo { get; set; }

    [CommandSwitch("--remote-repo-config-desc")]
    public string? RemoteRepoConfigDesc { get; set; }

    [CommandSwitch("--remote-username")]
    public string? RemoteUsername { get; set; }

    [CommandSwitch("--remote-yum-repo")]
    public string? RemoteYumRepo { get; set; }

    [CommandSwitch("--remote-yum-repo-path")]
    public string? RemoteYumRepoPath { get; set; }

    [CommandSwitch("--upstream-policy-file")]
    public string? UpstreamPolicyFile { get; set; }

    [CommandSwitch("--version-policy")]
    public string? VersionPolicy { get; set; }
}