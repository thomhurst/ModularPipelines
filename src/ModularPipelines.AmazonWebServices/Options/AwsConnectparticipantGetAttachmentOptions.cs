using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectparticipant", "get-attachment")]
public record AwsConnectparticipantGetAttachmentOptions(
[property: CommandSwitch("--attachment-id")] string AttachmentId,
[property: CommandSwitch("--connection-token")] string ConnectionToken
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}