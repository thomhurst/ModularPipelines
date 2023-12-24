using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("inspector2", "update-ec2-deep-inspection-configuration")]
public record AwsInspector2UpdateEc2DeepInspectionConfigurationOptions : AwsOptions
{
    [CommandSwitch("--package-paths")]
    public string[]? PackagePaths { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}