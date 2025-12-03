using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("communication", "chat", "thread", "create")]
public record AzCommunicationChatThreadCreateOptions(
[property: CliOption("--topic")] string Topic
) : AzOptions
{
    [CliOption("--access-token")]
    public string? AccessToken { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliOption("--idempotency-token")]
    public string? IdempotencyToken { get; set; }
}