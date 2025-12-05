using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appfabric", "update-ingestion-destination")]
public record AwsAppfabricUpdateIngestionDestinationOptions(
[property: CliOption("--app-bundle-identifier")] string AppBundleIdentifier,
[property: CliOption("--ingestion-identifier")] string IngestionIdentifier,
[property: CliOption("--ingestion-destination-identifier")] string IngestionDestinationIdentifier,
[property: CliOption("--destination-configuration")] string DestinationConfiguration
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}