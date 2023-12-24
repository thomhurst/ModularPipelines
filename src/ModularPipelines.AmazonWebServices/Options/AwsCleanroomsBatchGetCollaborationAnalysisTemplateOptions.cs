using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cleanrooms", "batch-get-collaboration-analysis-template")]
public record AwsCleanroomsBatchGetCollaborationAnalysisTemplateOptions(
[property: CommandSwitch("--collaboration-identifier")] string CollaborationIdentifier,
[property: CommandSwitch("--analysis-template-arns")] string[] AnalysisTemplateArns
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}