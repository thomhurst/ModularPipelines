using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("communication", "chat", "message", "receipt", "send")]
public record AzCommunicationChatMessageReceiptSendOptions(
[property: CommandSwitch("--message-id")] string MessageId,
[property: CommandSwitch("--thread")] string Thread
) : AzOptions
{
    [CommandSwitch("--access-token")]
    public string? AccessToken { get; set; }

    [CommandSwitch("--endpoint")]
    public string? Endpoint { get; set; }
}