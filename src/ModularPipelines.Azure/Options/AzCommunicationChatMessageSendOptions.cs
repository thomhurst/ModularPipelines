using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("communication", "chat", "message", "send")]
public record AzCommunicationChatMessageSendOptions(
[property: CommandSwitch("--content")] string Content,
[property: CommandSwitch("--thread")] string Thread
) : AzOptions
{
    [CommandSwitch("--access-token")]
    public string? AccessToken { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--endpoint")]
    public string? Endpoint { get; set; }

    [CommandSwitch("--message-type")]
    public string? MessageType { get; set; }
}