using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectparticipant", "get-attachment")]
public record AwsConnectparticipantGetAttachmentOptions(
[property: CliOption("--attachment-id")] string AttachmentId,
[property: CliOption("--connection-token")] string ConnectionToken
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}