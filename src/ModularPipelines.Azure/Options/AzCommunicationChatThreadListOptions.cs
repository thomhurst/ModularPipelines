using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("communication", "chat", "thread", "list")]
public record AzCommunicationChatThreadListOptions : AzOptions
{
    [CommandSwitch("--access-token")]
    public string? AccessToken { get; set; }

    [CommandSwitch("--endpoint")]
    public string? Endpoint { get; set; }

    [CommandSwitch("--start-time")]
    public string? StartTime { get; set; }
}