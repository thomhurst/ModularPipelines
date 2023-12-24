using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("shield", "associate-drt-log-bucket")]
public record AwsShieldAssociateDrtLogBucketOptions(
[property: CommandSwitch("--log-bucket")] string LogBucket
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}