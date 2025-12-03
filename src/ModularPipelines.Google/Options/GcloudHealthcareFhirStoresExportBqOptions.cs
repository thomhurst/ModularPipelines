using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "fhir-stores", "export", "bq")]
public record GcloudHealthcareFhirStoresExportBqOptions(
[property: CliArgument] string FhirStore,
[property: CliArgument] string Dataset,
[property: CliArgument] string Location,
[property: CliOption("--bq-dataset")] string BqDataset,
[property: CliOption("--schema-type")] string SchemaType
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--recursive-depth")]
    public string? RecursiveDepth { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }

    [CliOption("--since")]
    public string? Since { get; set; }

    [CliOption("--write-disposition")]
    public string? WriteDisposition { get; set; }
}