using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codepipeline", "put-job-failure-result")]
public record AwsCodepipelinePutJobFailureResultOptions(
[property: CommandSwitch("--job-id")] string JobId,
[property: CommandSwitch("--failure-details")] string FailureDetails
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}