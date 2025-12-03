using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("certificate-manager", "issuance-configs", "create")]
public record GcloudCertificateManagerIssuanceConfigsCreateOptions(
[property: CliArgument] string CertificateIssuanceConfig,
[property: CliArgument] string Location,
[property: CliOption("--ca-pool")] string CaPool
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--key-algorithm")]
    public string? KeyAlgorithm { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--lifetime")]
    public string? Lifetime { get; set; }

    [CliOption("--rotation-window-percentage")]
    public string? RotationWindowPercentage { get; set; }
}