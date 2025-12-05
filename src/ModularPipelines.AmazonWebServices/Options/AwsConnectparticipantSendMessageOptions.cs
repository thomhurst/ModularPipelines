using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectparticipant", "send-message")]
public record AwsConnectparticipantSendMessageOptions(
[property: CliOption("--content-type")] string ContentType,
[property: CliOption("--content")] string Content,
[property: CliOption("--connection-token")] string ConnectionToken
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}