using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("webpubsub", "service", "group", "remove-connection")]
public record AzWebpubsubServiceGroupRemoveConnectionOptions(
[property: CliOption("--connection-id")] string ConnectionId,
[property: CliOption("--group-name")] string GroupName,
[property: CliOption("--hub-name")] string HubName
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}