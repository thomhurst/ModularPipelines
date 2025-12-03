using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicebus", "topic", "subscription", "list")]
public record AzServicebusTopicSubscriptionListOptions(
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--topic-name")] string TopicName
) : AzOptions
{
    [CliOption("--skip")]
    public string? Skip { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}