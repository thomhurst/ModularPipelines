using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wisdom", "get-quick-response")]
public record AwsWisdomGetQuickResponseOptions(
[property: CliOption("--knowledge-base-id")] string KnowledgeBaseId,
[property: CliOption("--quick-response-id")] string QuickResponseId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}