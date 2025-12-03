using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("communication", "chat", "message", "send")]
public record AzCommunicationChatMessageSendOptions(
[property: CliOption("--content")] string Content,
[property: CliOption("--thread")] string Thread
) : AzOptions
{
    [CliOption("--access-token")]
    public string? AccessToken { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliOption("--message-type")]
    public string? MessageType { get; set; }
}