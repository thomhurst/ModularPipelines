using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("inspector2", "update-ec2-deep-inspection-configuration")]
public record AwsInspector2UpdateEc2DeepInspectionConfigurationOptions : AwsOptions
{
    [CliOption("--package-paths")]
    public string[]? PackagePaths { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}