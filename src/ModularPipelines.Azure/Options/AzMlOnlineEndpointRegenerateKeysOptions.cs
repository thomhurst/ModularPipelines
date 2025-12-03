using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "online-endpoint", "regenerate-keys")]
public record AzMlOnlineEndpointRegenerateKeysOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--key-type")]
    public string? KeyType { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}