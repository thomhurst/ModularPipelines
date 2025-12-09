using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "put-data-catalog-encryption-settings")]
public record AwsGluePutDataCatalogEncryptionSettingsOptions(
[property: CliOption("--data-catalog-encryption-settings")] string DataCatalogEncryptionSettings
) : AwsOptions
{
    [CliOption("--catalog-id")]
    public string? CatalogId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}