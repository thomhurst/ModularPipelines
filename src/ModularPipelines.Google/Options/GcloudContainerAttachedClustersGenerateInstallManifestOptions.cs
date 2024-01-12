using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "attached", "clusters", "generate-install-manifest")]
public record GcloudContainerAttachedClustersGenerateInstallManifestOptions(
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--platform-version")] string PlatformVersion
) : GcloudOptions
{
    [CommandSwitch("--output-file")]
    public string? OutputFile { get; set; }

    [CommandSwitch("--proxy-secret-name")]
    public string? ProxySecretName { get; set; }

    [CommandSwitch("--proxy-secret-namespace")]
    public string? ProxySecretNamespace { get; set; }
}