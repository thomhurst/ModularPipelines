using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("communication", "chat", "participant", "add")]
public record AzCommunicationChatParticipantAddOptions(
[property: CommandSwitch("--thread")] string Thread,
[property: CommandSwitch("--user")] string User
) : AzOptions
{
    [CommandSwitch("--access-token")]
    public string? AccessToken { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--endpoint")]
    public string? Endpoint { get; set; }

    [CommandSwitch("--start-time")]
    public string? StartTime { get; set; }
}