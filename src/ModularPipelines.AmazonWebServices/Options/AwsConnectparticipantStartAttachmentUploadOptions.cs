using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectparticipant", "start-attachment-upload")]
public record AwsConnectparticipantStartAttachmentUploadOptions(
[property: CliOption("--content-type")] string ContentType,
[property: CliOption("--attachment-size-in-bytes")] long AttachmentSizeInBytes,
[property: CliOption("--attachment-name")] string AttachmentName,
[property: CliOption("--connection-token")] string ConnectionToken
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}