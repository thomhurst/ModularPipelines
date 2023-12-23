using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codepipeline", "poll-for-third-party-jobs")]
public record AwsCodepipelinePollForThirdPartyJobsOptions(
[property: CommandSwitch("--action-type-id")] string ActionTypeId
) : AwsOptions
{
    [CommandSwitch("--max-batch-size")]
    public int? MaxBatchSize { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}