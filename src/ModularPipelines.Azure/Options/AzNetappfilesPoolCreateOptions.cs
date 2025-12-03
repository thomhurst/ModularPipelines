using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("netappfiles", "pool", "create")]
public record AzNetappfilesPoolCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-level")] string ServiceLevel,
[property: CliOption("--size")] string Size
) : AzOptions
{
    [CliFlag("--cool-access")]
    public bool? CoolAccess { get; set; }

    [CliOption("--encryption-type")]
    public string? EncryptionType { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--qos-type")]
    public string? QosType { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}