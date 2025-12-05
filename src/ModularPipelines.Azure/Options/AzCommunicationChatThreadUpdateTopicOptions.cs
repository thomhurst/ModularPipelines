using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("communication", "chat", "thread", "update-topic")]
public record AzCommunicationChatThreadUpdateTopicOptions(
[property: CliOption("--thread")] string Thread,
[property: CliOption("--topic")] string Topic
) : AzOptions
{
    [CliOption("--access-token")]
    public string? AccessToken { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }
}