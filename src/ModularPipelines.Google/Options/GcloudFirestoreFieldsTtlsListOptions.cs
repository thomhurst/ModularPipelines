using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firestore", "fields", "ttls", "list")]
public record GcloudFirestoreFieldsTtlsListOptions : GcloudOptions
{
    [CliOption("--collection-group")]
    public string? CollectionGroup { get; set; }

    [CliOption("--database")]
    public string? Database { get; set; }
}