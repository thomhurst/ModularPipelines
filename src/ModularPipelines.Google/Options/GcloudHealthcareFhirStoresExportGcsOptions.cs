using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcare", "fhir-stores", "export", "gcs")]
public record GcloudHealthcareFhirStoresExportGcsOptions(
[property: PositionalArgument] string FhirStore,
[property: PositionalArgument] string Dataset,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--gcs-uri")] string GcsUri
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }

    [CommandSwitch("--since")]
    public string? Since { get; set; }
}