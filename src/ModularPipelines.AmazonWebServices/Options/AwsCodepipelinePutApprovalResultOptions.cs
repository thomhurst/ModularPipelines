using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codepipeline", "put-approval-result")]
public record AwsCodepipelinePutApprovalResultOptions(
[property: CommandSwitch("--pipeline-name")] string PipelineName,
[property: CommandSwitch("--stage-name")] string StageName,
[property: CommandSwitch("--action-name")] string ActionName,
[property: CommandSwitch("--result")] string Result,
[property: CommandSwitch("--token")] string Token
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}