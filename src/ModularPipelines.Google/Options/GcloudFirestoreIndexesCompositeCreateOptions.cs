using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firestore", "indexes", "composite", "create")]
public record GcloudFirestoreIndexesCompositeCreateOptions(
[property: CliOption("--field-config")] string[] FieldConfig,
[property: CliOption("--collection-group")] string CollectionGroup,
[property: CliOption("--database")] string Database
) : GcloudOptions
{
    [CliOption("--api-scope")]
    public string? ApiScope { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--query-scope")]
    public string? QueryScope { get; set; }
}