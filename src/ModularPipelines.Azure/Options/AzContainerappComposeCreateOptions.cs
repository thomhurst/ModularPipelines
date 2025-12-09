using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "compose", "create")]
public record AzContainerappComposeCreateOptions(
[property: CliOption("--environment")] string Environment,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--compose-file-path")]
    public string? ComposeFilePath { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--registry-password")]
    public string? RegistryPassword { get; set; }

    [CliOption("--registry-server")]
    public string? RegistryServer { get; set; }

    [CliOption("--registry-username")]
    public string? RegistryUsername { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--transport-mapping")]
    public string? TransportMapping { get; set; }
}