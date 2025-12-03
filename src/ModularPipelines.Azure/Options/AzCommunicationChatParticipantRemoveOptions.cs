using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("communication", "chat", "participant", "remove")]
public record AzCommunicationChatParticipantRemoveOptions(
[property: CliOption("--thread")] string Thread,
[property: CliOption("--user")] string User
) : AzOptions
{
    [CliOption("--access-token")]
    public string? AccessToken { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}