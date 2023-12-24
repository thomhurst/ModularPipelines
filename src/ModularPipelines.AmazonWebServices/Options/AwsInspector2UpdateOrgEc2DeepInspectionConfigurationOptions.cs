using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("inspector2", "update-org-ec2-deep-inspection-configuration")]
public record AwsInspector2UpdateOrgEc2DeepInspectionConfigurationOptions(
[property: CommandSwitch("--org-package-paths")] string[] OrgPackagePaths
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}