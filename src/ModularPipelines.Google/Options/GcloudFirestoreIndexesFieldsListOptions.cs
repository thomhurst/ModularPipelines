using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firestore", "indexes", "fields", "list")]
public record GcloudFirestoreIndexesFieldsListOptions : GcloudOptions
{
    [CliOption("--collection-group")]
    public string? CollectionGroup { get; set; }

    [CliOption("--database")]
    public string? Database { get; set; }
}