using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("privateca", "pools", "create")]
public record GcloudPrivatecaPoolsCreateOptions(
[property: CliArgument] string CaPool,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--issuance-policy")]
    public string? IssuancePolicy { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliFlag("--publish-ca-cert")]
    public bool? PublishCaCert { get; set; }

    [CliFlag("--publish-crl")]
    public bool? PublishCrl { get; set; }

    [CliOption("--publishing-encoding-format")]
    public string? PublishingEncodingFormat { get; set; }

    [CliOption("--tier")]
    public string? Tier { get; set; }
}