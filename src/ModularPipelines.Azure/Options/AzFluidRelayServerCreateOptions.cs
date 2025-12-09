using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("fluid-relay", "server", "create")]
public record AzFluidRelayServerCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--key-identity")]
    public string? KeyIdentity { get; set; }

    [CliOption("--key-url")]
    public string? KeyUrl { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--provisioning-state")]
    public string? ProvisioningState { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}