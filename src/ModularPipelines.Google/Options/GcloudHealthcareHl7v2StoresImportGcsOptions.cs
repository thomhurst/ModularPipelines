using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcare", "hl7v2-stores", "import", "gcs")]
public record GcloudHealthcareHl7v2StoresImportGcsOptions(
[property: PositionalArgument] string HL7V2Store,
[property: PositionalArgument] string Dataset,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--gcs-uri")] string GcsUri
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}