using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("shield", "disassociate-drt-log-bucket")]
public record AwsShieldDisassociateDrtLogBucketOptions(
[property: CommandSwitch("--log-bucket")] string LogBucket
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}