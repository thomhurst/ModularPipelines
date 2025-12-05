using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appfabric", "create-ingestion")]
public record AwsAppfabricCreateIngestionOptions(
[property: CliOption("--app-bundle-identifier")] string AppBundleIdentifier,
[property: CliOption("--app")] string App,
[property: CliOption("--tenant-id")] string TenantId,
[property: CliOption("--ingestion-type")] string IngestionType
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}