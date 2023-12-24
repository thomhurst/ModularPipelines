using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "update-project")]
public record AwsSagemakerUpdateProjectOptions(
[property: CommandSwitch("--project-name")] string ProjectName
) : AwsOptions
{
    [CommandSwitch("--project-description")]
    public string? ProjectDescription { get; set; }

    [CommandSwitch("--service-catalog-provisioning-update-details")]
    public string? ServiceCatalogProvisioningUpdateDetails { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}