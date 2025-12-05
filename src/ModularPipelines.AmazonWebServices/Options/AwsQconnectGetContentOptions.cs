using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qconnect", "get-content")]
public record AwsQconnectGetContentOptions(
[property: CliOption("--content-id")] string ContentId,
[property: CliOption("--knowledge-base-id")] string KnowledgeBaseId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}