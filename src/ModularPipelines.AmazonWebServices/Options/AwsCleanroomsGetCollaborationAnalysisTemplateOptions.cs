using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cleanrooms", "get-collaboration-analysis-template")]
public record AwsCleanroomsGetCollaborationAnalysisTemplateOptions(
[property: CommandSwitch("--collaboration-identifier")] string CollaborationIdentifier,
[property: CommandSwitch("--analysis-template-arn")] string AnalysisTemplateArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}