using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("certificate-manager", "issuance-configs", "create")]
public record GcloudCertificateManagerIssuanceConfigsCreateOptions(
[property: PositionalArgument] string CertificateIssuanceConfig,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--ca-pool")] string CaPool
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--key-algorithm")]
    public string? KeyAlgorithm { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--lifetime")]
    public string? Lifetime { get; set; }

    [CommandSwitch("--rotation-window-percentage")]
    public string? RotationWindowPercentage { get; set; }
}