using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("privateca", "pools", "update")]
public record GcloudPrivatecaPoolsUpdateOptions(
[property: PositionalArgument] string CaPool,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--issuance-policy")]
    public string? IssuancePolicy { get; set; }

    [BooleanCommandSwitch("--publish-ca-cert")]
    public bool? PublishCaCert { get; set; }

    [BooleanCommandSwitch("--publish-crl")]
    public bool? PublishCrl { get; set; }

    [CommandSwitch("--publishing-encoding-format")]
    public string? PublishingEncodingFormat { get; set; }

    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }
}