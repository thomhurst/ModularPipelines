using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcare", "fhir-stores", "update")]
public record GcloudHealthcareFhirStoresUpdateOptions(
[property: PositionalArgument] string FhirStore,
[property: PositionalArgument] string Dataset,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--enable-update-create")]
    public bool? EnableUpdateCreate { get; set; }

    [CommandSwitch("--pubsub-topic")]
    public string? PubsubTopic { get; set; }
}