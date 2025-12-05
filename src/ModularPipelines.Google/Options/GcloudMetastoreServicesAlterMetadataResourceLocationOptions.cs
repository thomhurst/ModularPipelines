using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("metastore", "services", "alter-metadata-resource-location")]
public record GcloudMetastoreServicesAlterMetadataResourceLocationOptions(
[property: CliArgument] string Service,
[property: CliArgument] string Location,
[property: CliOption("--location_uri")] string LocationUri,
[property: CliOption("--resource_name")] string ResourceName
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}