using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectparticipant", "complete-attachment-upload")]
public record AwsConnectparticipantCompleteAttachmentUploadOptions(
[property: CommandSwitch("--attachment-ids")] string[] AttachmentIds,
[property: CommandSwitch("--connection-token")] string ConnectionToken
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}