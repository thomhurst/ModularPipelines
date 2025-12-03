using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lakeformation", "update-lake-formation-identity-center-configuration")]
public record AwsLakeformationUpdateLakeFormationIdentityCenterConfigurationOptions : AwsOptions
{
    [CliOption("--catalog-id")]
    public string? CatalogId { get; set; }

    [CliOption("--application-status")]
    public string? ApplicationStatus { get; set; }

    [CliOption("--external-filtering")]
    public string? ExternalFiltering { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}