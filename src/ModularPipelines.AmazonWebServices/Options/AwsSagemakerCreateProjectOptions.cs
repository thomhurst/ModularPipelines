using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-project")]
public record AwsSagemakerCreateProjectOptions(
[property: CommandSwitch("--project-name")] string ProjectName,
[property: CommandSwitch("--service-catalog-provisioning-details")] string ServiceCatalogProvisioningDetails
) : AwsOptions
{
    [CommandSwitch("--project-description")]
    public string? ProjectDescription { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}