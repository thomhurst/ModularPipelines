using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appfabric", "delete-ingestion-destination")]
public record AwsAppfabricDeleteIngestionDestinationOptions(
[property: CliOption("--app-bundle-identifier")] string AppBundleIdentifier,
[property: CliOption("--ingestion-identifier")] string IngestionIdentifier,
[property: CliOption("--ingestion-destination-identifier")] string IngestionDestinationIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}