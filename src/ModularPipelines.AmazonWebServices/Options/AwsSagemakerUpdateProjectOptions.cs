using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "update-project")]
public record AwsSagemakerUpdateProjectOptions(
[property: CliOption("--project-name")] string ProjectName
) : AwsOptions
{
    [CliOption("--project-description")]
    public string? ProjectDescription { get; set; }

    [CliOption("--service-catalog-provisioning-update-details")]
    public string? ServiceCatalogProvisioningUpdateDetails { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}