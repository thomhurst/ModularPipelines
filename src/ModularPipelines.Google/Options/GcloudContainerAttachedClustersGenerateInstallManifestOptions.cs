using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "attached", "clusters", "generate-install-manifest")]
public record GcloudContainerAttachedClustersGenerateInstallManifestOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Location,
[property: CliOption("--platform-version")] string PlatformVersion
) : GcloudOptions
{
    [CliOption("--output-file")]
    public string? OutputFile { get; set; }

    [CliOption("--proxy-secret-name")]
    public string? ProxySecretName { get; set; }

    [CliOption("--proxy-secret-namespace")]
    public string? ProxySecretNamespace { get; set; }
}