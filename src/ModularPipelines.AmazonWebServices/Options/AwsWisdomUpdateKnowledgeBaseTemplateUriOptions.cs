using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wisdom", "update-knowledge-base-template-uri")]
public record AwsWisdomUpdateKnowledgeBaseTemplateUriOptions(
[property: CliOption("--knowledge-base-id")] string KnowledgeBaseId,
[property: CliOption("--template-uri")] string TemplateUri
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}