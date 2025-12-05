using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "install")]
public record AwsDeployInstallOptions(
[property: CliOption("--config-file")] string ConfigFile
) : AwsOptions
{
    [CliFlag("--override-config")]
    public bool? OverrideConfig { get; set; }

    [CliOption("--agent-installer")]
    public string? AgentInstaller { get; set; }
}