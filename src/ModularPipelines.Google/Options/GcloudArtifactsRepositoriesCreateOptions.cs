using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "repositories", "create")]
public record GcloudArtifactsRepositoriesCreateOptions(
[property: CliArgument] string Repository,
[property: CliArgument] string Location,
[property: CliOption("--repository-format")] string RepositoryFormat
) : GcloudOptions
{
    [CliFlag("--allow-snapshot-overwrites")]
    public bool? AllowSnapshotOverwrites { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--immutable-tags")]
    public bool? ImmutableTags { get; set; }

    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--mode")]
    public string? Mode { get; set; }

    [CliOption("--remote-apt-repo")]
    public string? RemoteAptRepo { get; set; }

    [CliOption("--remote-apt-repo-path")]
    public string? RemoteAptRepoPath { get; set; }

    [CliOption("--remote-docker-repo")]
    public string? RemoteDockerRepo { get; set; }

    [CliOption("--remote-mvn-repo")]
    public string? RemoteMvnRepo { get; set; }

    [CliOption("--remote-npm-repo")]
    public string? RemoteNpmRepo { get; set; }

    [CliOption("--remote-password-secret-version")]
    public string? RemotePasswordSecretVersion { get; set; }

    [CliOption("--remote-python-repo")]
    public string? RemotePythonRepo { get; set; }

    [CliOption("--remote-repo-config-desc")]
    public string? RemoteRepoConfigDesc { get; set; }

    [CliOption("--remote-username")]
    public string? RemoteUsername { get; set; }

    [CliOption("--remote-yum-repo")]
    public string? RemoteYumRepo { get; set; }

    [CliOption("--remote-yum-repo-path")]
    public string? RemoteYumRepoPath { get; set; }

    [CliOption("--upstream-policy-file")]
    public string? UpstreamPolicyFile { get; set; }

    [CliOption("--version-policy")]
    public string? VersionPolicy { get; set; }
}