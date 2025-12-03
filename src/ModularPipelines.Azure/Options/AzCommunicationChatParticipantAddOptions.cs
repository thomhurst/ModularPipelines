using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("communication", "chat", "participant", "add")]
public record AzCommunicationChatParticipantAddOptions(
[property: CliOption("--thread")] string Thread,
[property: CliOption("--user")] string User
) : AzOptions
{
    [CliOption("--access-token")]
    public string? AccessToken { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }
}