using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wisdom", "delete-content")]
public record AwsWisdomDeleteContentOptions(
[property: CommandSwitch("--content-id")] string ContentId,
[property: CommandSwitch("--knowledge-base-id")] string KnowledgeBaseId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}