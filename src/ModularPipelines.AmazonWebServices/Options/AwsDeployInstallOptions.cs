using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "install")]
public record AwsDeployInstallOptions(
[property: CommandSwitch("--config-file")] string ConfigFile
) : AwsOptions
{
    [BooleanCommandSwitch("--override-config")]
    public bool? OverrideConfig { get; set; }

    [CommandSwitch("--agent-installer")]
    public string? AgentInstaller { get; set; }
}