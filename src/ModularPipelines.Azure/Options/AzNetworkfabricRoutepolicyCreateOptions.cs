using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkfabric", "routepolicy", "create")]
public record AzNetworkfabricRoutepolicyCreateOptions(
[property: CommandSwitch("--nf-id")] string NfId,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--resource-name")] string ResourceName,
[property: CommandSwitch("--statements")] string Statements
) : AzOptions
{
    [CommandSwitch("--address-family-type")]
    public string? AddressFamilyType { get; set; }

    [CommandSwitch("--annotation")]
    public string? Annotation { get; set; }

    [CommandSwitch("--default-action")]
    public string? DefaultAction { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}