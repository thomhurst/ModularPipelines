using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("communication", "chat", "message", "update")]
public record AzCommunicationChatMessageUpdateOptions(
[property: CliOption("--content")] string Content,
[property: CliOption("--message-id")] string MessageId,
[property: CliOption("--thread")] string Thread
) : AzOptions
{
    [CliOption("--access-token")]
    public string? AccessToken { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }
}