using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wisdom", "remove-knowledge-base-template-uri")]
public record AwsWisdomRemoveKnowledgeBaseTemplateUriOptions(
[property: CliOption("--knowledge-base-id")] string KnowledgeBaseId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}