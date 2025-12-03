using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("privateca", "subordinates", "update")]
public record GcloudPrivatecaSubordinatesUpdateOptions(
[property: CliArgument] string CertificateAuthority,
[property: CliArgument] string Location,
[property: CliArgument] string Pool
) : GcloudOptions
{
    [CliOption("--pem-chain")]
    public string? PemChain { get; set; }

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }
}