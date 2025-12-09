using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanrooms", "batch-get-collaboration-analysis-template")]
public record AwsCleanroomsBatchGetCollaborationAnalysisTemplateOptions(
[property: CliOption("--collaboration-identifier")] string CollaborationIdentifier,
[property: CliOption("--analysis-template-arns")] string[] AnalysisTemplateArns
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}