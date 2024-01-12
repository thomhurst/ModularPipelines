using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcare", "fhir-stores", "create")]
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

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string FhirStore { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Dataset { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Location { get; set; }

    [CommandSwitch("--version")]
    public new string Version { get; set; }

    [BooleanCommandSwitch("--disable-referential-integrity")]
    public bool? DisableReferentialIntegrity { get; set; }

    [BooleanCommandSwitch("--disable-resource-versioning")]
    public bool? DisableResourceVersioning { get; set; }

    [BooleanCommandSwitch("--enable-update-create")]
    public bool? EnableUpdateCreate { get; set; }

    [CommandSwitch("--pubsub-topic")]
    public string? PubsubTopic { get; set; }
}
