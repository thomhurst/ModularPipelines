using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qconnect", "get-quick-response")]
public record AwsQconnectGetQuickResponseOptions(
[property: CliOption("--knowledge-base-id")] string KnowledgeBaseId,
[property: CliOption("--quick-response-id")] string QuickResponseId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}