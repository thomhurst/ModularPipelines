using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudtrail", "describe-trails")]
public record AwsCloudtrailDescribeTrailsOptions : AwsOptions
{
    [CommandSwitch("--trail-name-list")]
    public string[]? TrailNameList { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}