using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "fhir-stores", "create")]
public record GcloudHealthcareFhirStoresCreateOptions : GcloudOptions
{
    public GcloudHealthcareFhirStoresCreateOptions(
        string fhirStore,
        string dataset,
        string location,
        string version
    )
    {
        FhirStore = fhirStore;
        Dataset = dataset;
        Location = location;
        Version = version;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string FhirStore { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Dataset { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Location { get; set; }

    [CliOption("--version")]
    public new string Version { get; set; }

    [CliFlag("--disable-referential-integrity")]
    public bool? DisableReferentialIntegrity { get; set; }

    [CliFlag("--disable-resource-versioning")]
    public bool? DisableResourceVersioning { get; set; }

    [CliFlag("--enable-update-create")]
    public bool? EnableUpdateCreate { get; set; }

    [CliOption("--pubsub-topic")]
    public string? PubsubTopic { get; set; }
}
