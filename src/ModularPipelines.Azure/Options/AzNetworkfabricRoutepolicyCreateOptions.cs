using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkfabric", "routepolicy", "create")]
public record AzNetworkfabricRoutepolicyCreateOptions(
[property: CliOption("--nf-id")] string NfId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--resource-name")] string ResourceName,
[property: CliOption("--statements")] string Statements
) : AzOptions
{
    [CliOption("--address-family-type")]
    public string? AddressFamilyType { get; set; }

    [CliOption("--annotation")]
    public string? Annotation { get; set; }

    [CliOption("--default-action")]
    public string? DefaultAction { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}