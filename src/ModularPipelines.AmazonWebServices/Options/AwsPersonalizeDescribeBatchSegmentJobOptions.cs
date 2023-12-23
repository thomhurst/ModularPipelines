using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("personalize", "describe-batch-segment-job")]
public record AwsPersonalizeDescribeBatchSegmentJobOptions(
[property: CommandSwitch("--batch-segment-job-arn")] string BatchSegmentJobArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}