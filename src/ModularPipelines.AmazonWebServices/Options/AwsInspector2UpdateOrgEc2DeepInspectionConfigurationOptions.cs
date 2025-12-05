using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("inspector2", "update-org-ec2-deep-inspection-configuration")]
public record AwsInspector2UpdateOrgEc2DeepInspectionConfigurationOptions(
[property: CliOption("--org-package-paths")] string[] OrgPackagePaths
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}