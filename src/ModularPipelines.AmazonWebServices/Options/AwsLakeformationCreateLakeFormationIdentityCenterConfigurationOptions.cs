using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lakeformation", "create-lake-formation-identity-center-configuration")]
public record AwsLakeformationCreateLakeFormationIdentityCenterConfigurationOptions : AwsOptions
{
    [CliOption("--catalog-id")]
    public string? CatalogId { get; set; }

    [CliOption("--instance-arn")]
    public string? InstanceArn { get; set; }

    [CliOption("--external-filtering")]
    public string? ExternalFiltering { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}