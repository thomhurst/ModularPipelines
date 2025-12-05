using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appfabric", "stop-ingestion")]
public record AwsAppfabricStopIngestionOptions(
[property: CliOption("--ingestion-identifier")] string IngestionIdentifier,
[property: CliOption("--app-bundle-identifier")] string AppBundleIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}