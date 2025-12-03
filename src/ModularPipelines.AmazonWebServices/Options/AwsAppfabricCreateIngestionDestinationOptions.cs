using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appfabric", "create-ingestion-destination")]
public record AwsAppfabricCreateIngestionDestinationOptions(
[property: CliOption("--app-bundle-identifier")] string AppBundleIdentifier,
[property: CliOption("--ingestion-identifier")] string IngestionIdentifier,
[property: CliOption("--processing-configuration")] string ProcessingConfiguration,
[property: CliOption("--destination-configuration")] string DestinationConfiguration
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}