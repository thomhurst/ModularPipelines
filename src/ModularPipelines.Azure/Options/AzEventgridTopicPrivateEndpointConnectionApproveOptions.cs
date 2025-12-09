using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventgrid", "topic", "private-endpoint-connection", "approve")]
public record AzEventgridTopicPrivateEndpointConnectionApproveOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--topic-name")] string TopicName
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }
}