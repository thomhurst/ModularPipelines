using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codepipeline", "put-approval-result")]
public record AwsCodepipelinePutApprovalResultOptions(
[property: CliOption("--pipeline-name")] string PipelineName,
[property: CliOption("--stage-name")] string StageName,
[property: CliOption("--action-name")] string ActionName,
[property: CliOption("--result")] string Result,
[property: CliOption("--token")] string Token
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}