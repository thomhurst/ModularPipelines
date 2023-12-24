using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "put-data-catalog-encryption-settings")]
public record AwsGluePutDataCatalogEncryptionSettingsOptions(
[property: CommandSwitch("--data-catalog-encryption-settings")] string DataCatalogEncryptionSettings
) : AwsOptions
{
    [CommandSwitch("--catalog-id")]
    public string? CatalogId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}