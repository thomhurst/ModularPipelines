using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-project")]
public record AwsSagemakerCreateProjectOptions(
[property: CliOption("--project-name")] string ProjectName,
[property: CliOption("--service-catalog-provisioning-details")] string ServiceCatalogProvisioningDetails
) : AwsOptions
{
    [CliOption("--project-description")]
    public string? ProjectDescription { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}