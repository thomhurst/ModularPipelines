using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("webpubsub", "service", "user", "send")]
public record AzWebpubsubServiceUserSendOptions(
[property: CliOption("--hub-name")] string HubName,
[property: CliOption("--payload")] string Payload,
[property: CliOption("--user-id")] string UserId
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