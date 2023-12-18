using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("communication", "chat", "thread", "update-topic")]
public record AzCommunicationChatThreadUpdateTopicOptions(
[property: CommandSwitch("--thread")] string Thread,
[property: CommandSwitch("--topic")] string Topic
) : AzOptions
{
    [CommandSwitch("--access-token")]
    public string? AccessToken { get; set; }

    [CommandSwitch("--endpoint")]
    public string? Endpoint { get; set; }
}