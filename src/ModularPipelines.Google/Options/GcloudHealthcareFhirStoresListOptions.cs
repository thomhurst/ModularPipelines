using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcare", "fhir-stores", "list")]
public record GcloudHealthcareFhirStoresListOptions(
[property: CommandSwitch("--dataset")] string Dataset,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;