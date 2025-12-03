using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("privateca", "pools", "update")]
public record GcloudPrivatecaPoolsUpdateOptions(
[property: CliArgument] string CaPool,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--issuance-policy")]
    public string? IssuancePolicy { get; set; }

    [CliFlag("--publish-ca-cert")]
    public bool? PublishCaCert { get; set; }

    [CliFlag("--publish-crl")]
    public bool? PublishCrl { get; set; }

    [CliOption("--publishing-encoding-format")]
    public string? PublishingEncodingFormat { get; set; }

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }
}