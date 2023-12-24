using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qconnect", "update-knowledge-base-template-uri")]
public record AwsQconnectUpdateKnowledgeBaseTemplateUriOptions(
[property: CommandSwitch("--knowledge-base-id")] string KnowledgeBaseId,
[property: CommandSwitch("--template-uri")] string TemplateUri
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}