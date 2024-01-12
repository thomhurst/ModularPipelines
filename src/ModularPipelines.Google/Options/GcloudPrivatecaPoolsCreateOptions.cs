using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("privateca", "pools", "create")]
public record GcloudPrivatecaPoolsCreateOptions(
[property: PositionalArgument] string CaPool,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--issuance-policy")]
    public string? IssuancePolicy { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [BooleanCommandSwitch("--publish-ca-cert")]
    public bool? PublishCaCert { get; set; }

    [BooleanCommandSwitch("--publish-crl")]
    public bool? PublishCrl { get; set; }

    [CommandSwitch("--publishing-encoding-format")]
    public string? PublishingEncodingFormat { get; set; }

    [CommandSwitch("--tier")]
    public string? Tier { get; set; }
}