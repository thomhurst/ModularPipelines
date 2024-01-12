using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcare", "fhir-stores", "export", "bq")]
public record GcloudHealthcareFhirStoresExportBqOptions(
[property: PositionalArgument] string FhirStore,
[property: PositionalArgument] string Dataset,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--bq-dataset")] string BqDataset,
[property: CommandSwitch("--schema-type")] string SchemaType
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--recursive-depth")]
    public string? RecursiveDepth { get; set; }

    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }

    [CommandSwitch("--since")]
    public string? Since { get; set; }

    [CommandSwitch("--write-disposition")]
    public string? WriteDisposition { get; set; }
}