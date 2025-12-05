using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("privateca", "roots", "update")]
public record GcloudPrivatecaRootsUpdateOptions(
[property: CliArgument] string CertificateAuthority,
[property: CliArgument] string Location,
[property: CliArgument] string Pool
) : GcloudOptions
{
    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }
}