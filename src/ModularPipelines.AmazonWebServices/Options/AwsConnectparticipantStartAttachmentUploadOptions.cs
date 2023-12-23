using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectparticipant", "start-attachment-upload")]
public record AwsConnectparticipantStartAttachmentUploadOptions(
[property: CommandSwitch("--content-type")] string ContentType,
[property: CommandSwitch("--attachment-size-in-bytes")] long AttachmentSizeInBytes,
[property: CommandSwitch("--attachment-name")] string AttachmentName,
[property: CommandSwitch("--connection-token")] string ConnectionToken
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}