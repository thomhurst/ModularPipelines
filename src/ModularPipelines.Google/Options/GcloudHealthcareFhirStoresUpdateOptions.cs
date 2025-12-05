using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "fhir-stores", "update")]
public record GcloudHealthcareFhirStoresUpdateOptions(
[property: CliArgument] string FhirStore,
[property: CliArgument] string Dataset,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--enable-update-create")]
    public bool? EnableUpdateCreate { get; set; }

    [CliOption("--pubsub-topic")]
    public string? PubsubTopic { get; set; }
}