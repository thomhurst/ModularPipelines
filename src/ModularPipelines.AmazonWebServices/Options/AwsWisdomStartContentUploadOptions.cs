using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wisdom", "start-content-upload")]
public record AwsWisdomStartContentUploadOptions(
[property: CommandSwitch("--content-type")] string ContentType,
[property: CommandSwitch("--knowledge-base-id")] string KnowledgeBaseId
) : AwsOptions
{
    [CommandSwitch("--presigned-url-time-to-live")]
    public int? PresignedUrlTimeToLive { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}