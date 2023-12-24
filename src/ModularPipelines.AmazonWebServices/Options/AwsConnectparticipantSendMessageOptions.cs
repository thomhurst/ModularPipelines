using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectparticipant", "send-message")]
public record AwsConnectparticipantSendMessageOptions(
[property: CommandSwitch("--content-type")] string ContentType,
[property: CommandSwitch("--content")] string Content,
[property: CommandSwitch("--connection-token")] string ConnectionToken
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}