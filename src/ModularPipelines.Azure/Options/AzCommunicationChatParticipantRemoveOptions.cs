using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("communication", "chat", "participant", "remove")]
public record AzCommunicationChatParticipantRemoveOptions(
[property: CommandSwitch("--thread")] string Thread,
[property: CommandSwitch("--user")] string User
) : AzOptions
{
    [CommandSwitch("--access-token")]
    public string? AccessToken { get; set; }

    [CommandSwitch("--endpoint")]
    public string? Endpoint { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}