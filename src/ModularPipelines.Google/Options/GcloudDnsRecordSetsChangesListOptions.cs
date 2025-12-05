using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns", "record-sets", "changes", "list")]
public record GcloudDnsRecordSetsChangesListOptions(
[property: CliOption("--zone")] string Zone
) : GcloudOptions
{
    [CliOption("--sort-order")]
    public string? SortOrder { get; set; }
}