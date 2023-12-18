using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "compose", "create")]
public record AzContainerappComposeCreateOptions(
[property: CommandSwitch("--environment")] string Environment,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--compose-file-path")]
    public string? ComposeFilePath { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--registry-password")]
    public string? RegistryPassword { get; set; }

    [CommandSwitch("--registry-server")]
    public string? RegistryServer { get; set; }

    [CommandSwitch("--registry-username")]
    public string? RegistryUsername { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--transport-mapping")]
    public string? TransportMapping { get; set; }
}