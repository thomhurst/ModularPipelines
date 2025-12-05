using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectparticipant", "complete-attachment-upload")]
public record AwsConnectparticipantCompleteAttachmentUploadOptions(
[property: CliOption("--attachment-ids")] string[] AttachmentIds,
[property: CliOption("--connection-token")] string ConnectionToken
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}