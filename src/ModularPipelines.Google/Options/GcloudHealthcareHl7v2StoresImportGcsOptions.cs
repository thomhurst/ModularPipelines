using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "hl7v2-stores", "import", "gcs")]
public record GcloudHealthcareHl7v2StoresImportGcsOptions(
[property: CliArgument] string HL7V2Store,
[property: CliArgument] string Dataset,
[property: CliArgument] string Location,
[property: CliOption("--gcs-uri")] string GcsUri
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}